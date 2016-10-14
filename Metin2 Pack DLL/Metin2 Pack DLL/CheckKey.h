#ifndef __CHECK_KEY__
#define __CHECK_KEY__

#include "Decrypter.h"

#define LZ_KEY 0x5A4F434D

// Decompress function in the client
__declspec(naked) extern void ASM_LZO_CHECKKEY()
{
	__asm
	{
		MOV EAX,DWORD PTR SS:[ESP + 0x10]
		MOV ECX, EAX
		AND ECX, 0x80000007
	JNG LABEL1
		DEC ECX
		OR ECX, 0xFFFFFFF8
		INC ECX
LABEL1:
	JE LABEL2
		SUB EAX, ECX
		ADD EAX, 8
		MOV DWORD PTR SS:[ESP + 0x10],EAX
	JMP LABEL3;
LABEL2:
		MOV DWORD PTR SS:[ESP + 0x10],EAX
LABEL3:
		PUSH EBX
		MOV EBX, EAX
		SAR EBX, 0x03
		TEST EBX, EBX
	JLE LABEL5

		PUSH EBP
		//MOV EBP, lzoData
		MOV EBP, [ESP + 0x14]

		PUSH ESI
		//MOV ESI, inData
		MOV ESI, [ESP + 0x14]

		PUSH EDI
		//MOV EDI, outBuffer
		MOV EDI, [ESP + 0x14]
LABEL4:
		MOV EAX,DWORD PTR DS:[ESI]
		MOV ECX,[ESI + 0x04]
		PUSH EDI
		PUSH EBP
		PUSH EAX
		PUSH ECX
	CALL ASM_LZO_FUNC1
		ADD ESP, 0x10
		ADD EDI, 0x08
		ADD ESI, 0x08
		DEC EBX
	JNZ LABEL4
		MOV EAX,DWORD PTR SS:[ESP + 0x20]
		POP EDI
		POP ESI
		POP EBP
LABEL5:
		POP EBX
		RET
	}
}

extern int LZObject_CheckKey(LPBYTE outBuffer, LPBYTE inData, LPBYTE lzoData, DWORD dwSize)
{
	int result = 1;
	__asm
	{
		mov edx, dwSize

		mov ecx, inData
		sub ecx, 4

		mov eax, lzoData

		mov edi, outBuffer

		push edx
		push eax
		push ecx
		push edi

		call ASM_LZO_CHECKKEY

		MOV EDX, DWORD PTR DS:[EDI]
		MOV EAX, LZ_KEY
		ADD ESP, 0x10
		CMP EDX, EAX
		JE LABEL1
		mov result, 0
LABEL1:
		NOP
	}
	return result;
}

#endif