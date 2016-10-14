#ifndef __SAVEFILE__
#define __SAVEFILE__

#include "stdafx.h"

#include "SaveFile.h"
#include "Tokenizer.h"

// Saves the file to a specific folder based on the path. The root 
// directory I choose to use is named 'output'.
void SaveFile(const char * originalFilename, LPBYTE outBuffer, DWORD outBufferSize)
{
	std::stringstream dirPath;
	std::string error;
	std::vector<std::string> pathTokens = TokenizeString(originalFilename, "\\/");

	dirPath << "output";
	CreateDirectoryA(dirPath.str().c_str(), NULL);
	dirPath << "\\";

	size_t index = 0;
	for(index = 0; index < pathTokens.size() - 1; ++index)
	{
		if(pathTokens[index].find_first_of(":") != std::string::npos)
			continue;
		dirPath << pathTokens[index];
		CreateDirectoryA(dirPath.str().c_str(), NULL);
		dirPath << "\\";
	}

	dirPath << pathTokens[index];

	FILE * of = fopen(dirPath.str().c_str(), "wb");
	if(of)
	{
		fwrite(outBuffer, 1, outBufferSize, of);
		fclose(of);
	}
	else
	{
		error += ("Could not save the file %s\n", dirPath.str().c_str());
	}
}

#endif