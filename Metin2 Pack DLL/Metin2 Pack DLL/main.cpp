/*
	Metin2FileExtractor
	pushedx
	edxLabs

	This program serves as a file extractor for the Metin2 data files. The
	EIX files are the header files and the EPK files are the data files.
	The data can be uncompressed, encrypted, and/or compressed. This code
	shows how the client performs the logic.

	This was a "for fun" project and done in about ~12 hours of work. I am
	releasing this tool and the source code to the elitepvperss' Metin2 
	community to help spread new knowledge. I have no immediate plans for 
	a file editor at this time as I'm not playing the game or doing
	anything with it. Maybe sometime later though.

	I hope you enjoy!
*/

#define _CRT_SECURE_NO_WARNINGS
#include "stdafx.h"

#include "SaveFile.h"
#include "CheckKey.h"
#include "Decompresser.h"

//--------------------------------------------------------------------------


BOOL DllMain( HANDLE hModule, DWORD ul_reason_for_call, LPVOID lpReserved )
{
    return TRUE;
}

//--------------------------------------------------------------------------

struct TEntry1
{
	DWORD index;
	char filename[160];
	DWORD dw1;
	DWORD dw2;
	DWORD dw3;
	DWORD dwSrcSize;
	DWORD unpackedCRC;
	DWORD dwFileOffset;
	BYTE packedType;
	BYTE b2;
	BYTE b3;
	BYTE b4;
};

struct TEntry2
{
	DWORD header;
	DWORD decryptedBlockSize;
	DWORD compressedBlockSize;
	DWORD decompressedBlockSize;
};

struct TEntry3
{
	DWORD header;
	DWORD version;
	DWORD fileCount;
};

//--------------------------------------------------------------------------

// For decompressing (ripped from client)
BYTE gLZOData[] = 
{
	0xB9, 0x9E, 0xB0, 0x02, 0x6F, 0x69, 0x81, 0x05, 
	0x63, 0x98, 0x9B, 0x28, 0x79, 0x18, 0x1A, 0x00, 
};

// For decrypting (ripped from client)
BYTE gLZOData2[] = 
{
	0x22, 0xB8, 0xB4, 0x04, 0x64, 0xB2, 0x6E, 0x1F, 
	0xAE, 0xEA, 0x18, 0x00, 0xA6, 0xF6, 0xFB, 0x1C, 
};

// Dumps a complete archive. I have combined two sets of logic for this,
// but you can separate them if you want a more unique tool that allows
// you to extract individual files or explore the contents.
extern "C" __declspec(dllexport) bool DumpArchive(void)
{
	const char * name = "BGM";
	const char * inFolder = "C:\\Users\\Christian\\Desktop\\GFO\\Game Files Open - Metin2 Global Tools\\Game Files Open - Metin2 Global Tools\\bin\\Debug";

	HANDLE eixHandle = INVALID_HANDLE_VALUE;
	HANDLE epkHandle = INVALID_HANDLE_VALUE;
	SECURITY_ATTRIBUTES eixSecurity = {0};
	SECURITY_ATTRIBUTES epkSecurity = {0};
	DWORD eixFileSize = 0;
	DWORD epkFileSize = 0;
	HANDLE eixFileMapping = NULL;
	HANDLE epkFileMapping = NULL;
	LPBYTE eixFileBufferPtr = NULL;
	LPBYTE epkFileBufferPtr = NULL;
	bool bWasError = false;

	std::string eixName_ = inFolder;
	if(!(eixName_[eixName_.size() - 1] == '\\' || eixName_[eixName_.size() - 1] == '/'))
	{
		eixName_ += "\\";
	}
	eixName_ += name;
	eixName_ += ".eix";

	std::string epkName_ = inFolder;
	if(!(epkName_[epkName_.size() - 1] == '\\' || epkName_[epkName_.size() - 1] == '/'))
	{
		epkName_ += "\\";
	}
	epkName_ += name;
	epkName_ += ".epk";

	const char * eixName = eixName_.c_str();
	const char * epkName = epkName_.c_str();

	eixSecurity.nLength = sizeof(SECURITY_ATTRIBUTES);
	eixSecurity.bInheritHandle = TRUE;
	eixSecurity.lpSecurityDescriptor = NULL;

	epkSecurity.nLength = sizeof(SECURITY_ATTRIBUTES);
	epkSecurity.bInheritHandle = TRUE;
	epkSecurity.lpSecurityDescriptor = NULL;

	std::string error = "";

	FILE * err = fopen("syslog.txt", "wb");

	// Open the files to access
	while(bWasError == false)
	{
		eixHandle = CreateFileA(eixName, GENERIC_READ, FILE_SHARE_READ, &eixSecurity, OPEN_EXISTING, FILE_ATTRIBUTE_NORMAL, NULL);
		if(eixHandle == INVALID_HANDLE_VALUE)
		{
			error += ("Could not open the %s file. The program will now exit.\n", eixName);
			bWasError = true;
			break;
		}

		epkHandle = CreateFileA(epkName, GENERIC_READ, FILE_SHARE_READ, &epkSecurity, OPEN_EXISTING, FILE_ATTRIBUTE_NORMAL, NULL);
		if(epkHandle == INVALID_HANDLE_VALUE)
		{
			error +=("Could not open the %s file. The program will now exit.\n", epkName);
			bWasError = true;
			break;
		}
		break; // All done now
	}

	// Create the access handles for reading the files
	while(bWasError == false)
	{
		// We need to store the size of the file for file mapping
		eixFileSize = GetFileSize(eixHandle, NULL);
		if(eixFileSize == INVALID_FILE_SIZE)
		{
			DWORD dwError = GetLastError();
			if(dwError != NO_ERROR)
			{
				error +=("There was an error [%i] getting the file size of the %s file. The program will now exit.\n", dwError, eixName);
				bWasError = true;
				break;
			}
		}

		// We need to store the size of the file for file mapping
		epkFileSize = GetFileSize(epkHandle, NULL);
		if(epkFileSize == INVALID_FILE_SIZE)
		{
			DWORD dwError = GetLastError();
			if(dwError != NO_ERROR)
			{
				error +=("There was an error [%i] getting the file size of the %s file. The program will now exit.\n", dwError, epkName);
				bWasError = true;
				break;
			}
		}

		// Create a file mapping object
		eixFileMapping = CreateFileMapping(eixHandle, NULL, PAGE_READONLY, 0, eixFileSize, NULL);
		if(eixFileMapping == 0)
		{
			error +=("Could not create a file mapping object for the %s file. The program will now exit.\n", eixName);
			bWasError = true;
			break;
		}

		// Create a file mapping object
		epkFileMapping = CreateFileMapping(epkHandle, NULL, PAGE_READONLY, 0, epkFileSize, NULL);
		if(epkFileMapping == 0)
		{
			error +=("Could not create a file mapping object for the %s file. The program will now exit.\n", epkName);
			bWasError = true;
			break;
		}

		// Create a file mapping view
		eixFileBufferPtr = reinterpret_cast<LPBYTE>(MapViewOfFile(eixFileMapping, FILE_MAP_READ, 0, 0, eixFileSize));
		if(eixFileBufferPtr == 0)
		{
			error +=("Could not create a view of the the %s file. The program will now exit.\n", eixName);
			bWasError = true;
			break;
		}

		// Create a file mapping view
		epkFileBufferPtr = reinterpret_cast<LPBYTE>(MapViewOfFile(epkFileMapping, FILE_MAP_READ, 0, 0, epkFileSize));
		if(epkFileBufferPtr == 0)
		{
			error +=("Could not create a view of the the %s file. The program will now exit.\n", epkName);
			bWasError = true;
			break;
		}

		break; // All done now
	}

	// Now we need to verify the files we just loaded
	while(bWasError == false)
	{
		// We need at least 12 bytes
		if(eixFileSize < 0x0C)
		{
			error +=("The file size for the %s file is too small. The program will now exit.\n", eixName);
			bWasError = true;
			break;
		}

		// Verify the header
		LPDWORD eixHeader = reinterpret_cast<LPDWORD>(eixFileBufferPtr);
		if(*eixHeader != 0x444B5045) // Some hard coded check
		{
			// Important: This value is read from the client itself. If the client updates, this value
			// would need to be updated as well if it ever changed.
			if(*eixHeader != LZ_KEY)
			{
				error +=("The file header for the %s file is incorrect. The program will now exit.\n", eixName);
				bWasError = true;
				break;
			}

			// Get a file header pointer from the buffer
			TEntry2 * eixHeader = (TEntry2 *)eixFileBufferPtr;

			// Store a pointer to the encrypted data
			LPBYTE eixDataBuffer = eixFileBufferPtr + 0x14;

			// We don't care about this check because we will create the
			// buffers ourselves in dynamic memory. The game wants to be as
			// efficient as possible though.
			if(eixHeader->decompressedBlockSize <= 0x10000)
			{
			}

			// Allocate space for the decompressed buffer
			LPBYTE decompressedBuffer = new BYTE[eixHeader->decompressedBlockSize];
			memset(decompressedBuffer, 0, eixHeader->decompressedBlockSize);

			// If the contents of the file are not encrypted (no test data yet)
			// So, not going to add the implementation at this time.
			if(eixHeader->decryptedBlockSize == 0)
			{
				error +=("[TODO] -- eixHeader of %s is not encrypted!\n", eixName);
				bWasError = true;
				delete [] decompressedBuffer;
				break;
			}

			// We don't care about this check because we will create the
			// buffers ourselves in dynamic memory. The game wants to be as
			// efficient as possible though.
			if(eixHeader->decryptedBlockSize < 0x2000)
			{
			}

			// Create a buffer to decrypt the contents of the exi header into
			LPBYTE compressedBuffer = new BYTE[eixHeader->decryptedBlockSize];
			memset(compressedBuffer, 0, eixHeader->decryptedBlockSize);

			// Try to decrypt the data
			int result = LZObject_CheckKey(compressedBuffer, eixDataBuffer, gLZOData, eixHeader->decryptedBlockSize);
			if(result == 0)
			{
				delete [] decompressedBuffer;
				delete [] compressedBuffer;
				error +=("There was an error decrypting the data of the %s file. The program will now exit.\n", eixName);
				bWasError = true;
				break;
			}

			// Try to decompress the data now
			DWORD finalSize = 0;
			LZObject_Decompress(compressedBuffer, eixHeader->compressedBlockSize, decompressedBuffer, &finalSize, 0);

			// Make sure the file size matches
			if(finalSize != eixHeader->decompressedBlockSize)
			{
				delete [] decompressedBuffer;
				delete [] compressedBuffer;
				error +=("There was an error decompressing the data of the %s file. The program will now exit.\n", eixName);
				bWasError = true;
				break;
			}

			// Get a pointer to the new file header
			TEntry3 * entry3 = (TEntry3 *)decompressedBuffer;

			// Check the file version
			if(entry3->version != 2)
			{
				delete [] decompressedBuffer;
				delete [] compressedBuffer;
				error +=("The version of the %s file is incorrect. Expected (%i) Actual (%i). The program will now exit.\n", eixName, 2, entry3->version);
				bWasError = true;
				break;
			}

			// Make sure we have a match in the number of entries and the expected block size
			if(finalSize != (((entry3->fileCount + entry3->fileCount * 2) << 0x06) + 0x0C))
			{
				delete [] decompressedBuffer;
				delete [] compressedBuffer;
				error +=("The pack index file size of the %s file is incorrect. The program will now exit.\n", eixName);
				bWasError = true;
				break;
			}

			// Store a pointer to the block of data
			LPBYTE decompressedBlock = decompressedBuffer + 0x0C;

			// If we have files to process
			if(entry3->fileCount > 0)
			{
				// Build a filename for our dump file
				std::string dumpFileName = eixName;
				dumpFileName = dumpFileName.substr(1 + dumpFileName.find_last_of("\\/"));
				dumpFileName = dumpFileName.substr(0, dumpFileName.find_first_of("."));
				dumpFileName += "_dump.txt";

				// Create the output file for the eix header dump
				FILE * of = fopen(dumpFileName.c_str(), "w");
				if(of == 0)
				{
					error +=("There was an error creating the %s file. The header data will not be dumped.\n", dumpFileName.c_str());
				}

				// Loop through all of the files
				for(DWORD x = 0; x < entry3->fileCount; ++x)
				{
					// Create a pointer to the file entry block
					TEntry1 * pEntry = (TEntry1 *)decompressedBlock;

					// Make sure there is a value here
					if(pEntry->dw2 == 0)
					{
						error +=("No dw2 field set for the file %s\n", pEntry->filename);
						continue;
					}

					// Simple entry dump
					if(of)
					{
						fprintf(of, "%i. ", pEntry->index);
						fprintf(of, "%s\n", pEntry->filename);
						fprintf(of, "[%.8X]", pEntry->dw1);
						fprintf(of, "[%.8X]", pEntry->dw2);
						fprintf(of, "[%.8X]", pEntry->dw3);
						fprintf(of, "[%.8X]", pEntry->dwSrcSize);
						fprintf(of, "[%.8X]", pEntry->unpackedCRC);
						fprintf(of, "[%.8X]", pEntry->dwFileOffset);
						fprintf(of, "[%.2X %.2X %.2X %.2X]", pEntry->packedType, pEntry->b2, pEntry->b3, pEntry->b4);
						fprintf(of, "\n");
					}

					// Not compressed, no extra header
					if(pEntry->packedType == 0)
					{
						SaveFile(pEntry->filename, epkFileBufferPtr + pEntry->dwFileOffset, pEntry->dwSrcSize);
					}

					// Header and compressed/encrypted
					else
					{
						// Decompress
						if(pEntry->packedType == 1)
						{
							// Calculate the data pointer to the entry data block
							LPBYTE pDataPtr = epkFileBufferPtr + pEntry->dwFileOffset;

							// Get a pointer to the header for this block
							TEntry2 * pEntryHeader = (TEntry2*)pDataPtr;

							// Make sure the header is correct
							DWORD h = *(reinterpret_cast<LPDWORD>(pDataPtr));
							if(h != LZ_KEY)
							{
								error +=("The header for %s is incorrect. Expected (%x) Actual (%x).\n", pEntry->filename, LZ_KEY, h);
								continue;
							}

							// Allocate memory for the uncompressed data
							LPBYTE uncompressedData = new BYTE[pEntryHeader->decompressedBlockSize];
							memset(uncompressedData, 0, pEntryHeader->decompressedBlockSize);

							// Decompress the data
							LZObject_Decompress(pDataPtr + 16, pEntryHeader->compressedBlockSize, uncompressedData, &finalSize, 0);

							// Make sure the operation went right
							if(finalSize != pEntryHeader->decompressedBlockSize)
							{
								error +=("File size for %s differs from expected. Expected (%i) Actual (%i).\n", pEntry->filename, pEntryHeader->decompressedBlockSize, finalSize);
							}
							else
							{
								SaveFile(pEntry->filename, uncompressedData, pEntryHeader->decompressedBlockSize);
							}

							// Cleanup now
							delete [] uncompressedData;
						}

						// Decrypt + Decompress
						else if(pEntry->packedType == 2)
						{
							// Calculate the data pointer to the entry data block
							LPBYTE pDataPtr = epkFileBufferPtr + pEntry->dwFileOffset;

							// Get a pointer to the header for this block
							TEntry2 * pEntryHeader = (TEntry2*)pDataPtr;

							DWORD h = *(reinterpret_cast<LPDWORD>(pDataPtr));
							if(h != LZ_KEY)
							{
								error +=("The header for %s is incorrect. Expected (%x) Actual (%x).\n", pEntry->filename, LZ_KEY, h);
								continue;
							}

							// Create a buffer for the decrypted data
							LPBYTE decryptedData = new BYTE[pEntryHeader->decryptedBlockSize];
							memset(decryptedData, 0, pEntryHeader->decryptedBlockSize);

							// Decrypt the data
							int result = LZObject_CheckKey(decryptedData, pDataPtr + 20, gLZOData2, pEntryHeader->decryptedBlockSize);
							if(result == 0)
							{
								error +=("There was an error decrypting the data for the %s file. It will be skipped.\n", pEntry->filename);
								delete [] decryptedData;
								continue;
							}

							// Create a buffer for the final uncompressed data
							LPBYTE uncompressedData = new BYTE[pEntryHeader->decompressedBlockSize];
							memset(uncompressedData, 0, pEntryHeader->decompressedBlockSize);

							// Decompress
							LZObject_Decompress(decryptedData, pEntryHeader->compressedBlockSize, uncompressedData, &finalSize, 0);

							// Make sure the file sizes match
							if(finalSize != pEntryHeader->decompressedBlockSize)
							{
								error +=("File size for %s differs from expected. Expected (%i) Actual (%i)\n", pEntry->filename, pEntryHeader->decompressedBlockSize, finalSize);
							}
							else
							{
								SaveFile(pEntry->filename, uncompressedData, pEntryHeader->decompressedBlockSize);
							}

							// Cleanup now
							delete [] decryptedData;
							delete [] uncompressedData;
						}
					}
					decompressedBlock += 0xC0; // next block
				}

				// Close our output file for the header dump file
				if(of)
				{
					fclose(of);
				}
			}

			// Cleanup
			delete [] decompressedBuffer;
			delete [] compressedBuffer;
		}
		else
		{
			// If we get here, we are using an unsupported type of file that
			// was not accessible at the time this tool was written.
			error +=("[TODO] -- if(*eixHeader != 0x444B5045)\n");
			bWasError = true;
			break;
		}

		break; // All done now
	}

	err = fopen("syslog.txt", "wb");
	if(err)
	{
		fwrite(error.c_str(), 1, error.length() - 1, err);
		fclose(err);
	}

	// Cleanup

	if(eixFileMapping != 0)
		CloseHandle(eixFileMapping);
	if(epkFileMapping != 0)
		CloseHandle(epkFileMapping);

	if(eixHandle != INVALID_HANDLE_VALUE)
		CloseHandle(eixHandle);
	if(epkHandle != INVALID_HANDLE_VALUE)
		CloseHandle(epkHandle);

	// Return the status
	return (bWasError == false);
}

//--------------------------------------------------------------------------
