#ifndef __DECOMPRESSER__
#define __DECOMPRESSER__

#include "stdafx.h"

// Ripped from the client via OllyDbg. It was tedious, but simple work since
// you can set labels in OllyDbg for the new jump locations.
__declspec(naked) extern void ASM_LZO_DECOMPRESS()
{
	__asm
	{
		MOV EAX,DWORD PTR SS:[ESP+0x08]
		PUSH EBX
		MOV EBX,DWORD PTR SS:[ESP+0x14]
		PUSH EBP
		PUSH ESI
		MOV ESI,DWORD PTR SS:[ESP+0x10]
		MOV DWORD PTR DS:[EBX],0x00
		PUSH EDI
		MOV CL,BYTE PTR DS:[ESI]
		LEA EBP,DWORD PTR DS:[ESI+EAX]
		MOV EAX,DWORD PTR SS:[ESP+0x1C]
		CMP CL,0x11
	JBE label1
		AND ECX,0xFF
		SUB ECX,0x11
		INC ESI
		CMP ECX,0x04
	JB label2
label3:
		MOV DL,BYTE PTR DS:[ESI]
		MOV BYTE PTR DS:[EAX],DL
		INC EAX
		INC ESI
		DEC ECX
	JNZ label3
	JMP label4
label1:
		XOR ECX,ECX
		MOV CL,BYTE PTR DS:[ESI]
		INC ESI
		CMP ECX,0x10
	JNB label5
		TEST ECX,ECX
	JNZ label6
		CMP BYTE PTR DS:[ESI],0x00
	JNZ label7
label8:
		MOV DL,BYTE PTR DS:[ESI+0x01]
		ADD ECX,0xFF
		INC ESI
		TEST DL,DL
	JE label8
label7:
		XOR EDX,EDX
		MOV DL,BYTE PTR DS:[ESI]
		INC ESI
		LEA ECX,DWORD PTR DS:[ECX+EDX+0x0F]
label6:
		MOV EDX,DWORD PTR DS:[ESI]
		ADD ESI,0x04
		MOV DWORD PTR DS:[EAX],EDX
		ADD EAX,0x04
		DEC ECX                                             //  Switch (cases 1..4)
	JE label4
		CMP ECX,0x04
	JB label9
label10:
		MOV EDX,DWORD PTR DS:[ESI]                         //  Default case of switch 0055BACA
		SUB ECX,0x04
		MOV DWORD PTR DS:[EAX],EDX
		ADD EAX,0x04
		ADD ESI,0x04
		CMP ECX,0x04
	JNB label10
		TEST ECX,ECX
	JBE label4
label11:
		MOV DL,BYTE PTR DS:[ESI]
		MOV BYTE PTR DS:[EAX],DL
		INC EAX
		INC ESI
		DEC ECX
	JNZ label11
	JMP label4
label9:
		MOV DL,BYTE PTR DS:[ESI]                           //  Cases 2,3,4 of switch 0055BACA
		MOV BYTE PTR DS:[EAX],DL
		INC EAX
		INC ESI
		DEC ECX
	JNZ label9
label4:
		XOR ECX,ECX                                         //  Case 1 of switch 0055BACA
		MOV CL,BYTE PTR DS:[ESI]
		INC ESI
		CMP ECX,0x10
	JNB label5
		SHR ECX,0x02
		MOV EDX,EAX
		SUB EDX,ECX
		XOR ECX,ECX
		MOV CL,BYTE PTR DS:[ESI]
		SHL ECX,0x02
		SUB EDX,ECX
		MOV CL,BYTE PTR DS:[EDX-0x801]
		SUB EDX,0x0801
		INC ESI
		MOV BYTE PTR DS:[EAX],CL
		INC EAX
		INC EDX
lable28:
		MOV CL,BYTE PTR DS:[EDX]
		MOV BYTE PTR DS:[EAX],CL
		MOV DL,BYTE PTR DS:[EDX+0x01]
		INC EAX
		MOV BYTE PTR DS:[EAX],DL
		INC EAX
label14:
		MOV CL,BYTE PTR DS:[ESI-0x02]
		AND ECX,0x03
	JE label1
label2:
		MOV DL,BYTE PTR DS:[ESI]
		MOV BYTE PTR DS:[EAX],DL
		INC EAX
		INC ESI
		DEC ECX
	JNZ label2
		XOR ECX,ECX
		MOV CL,BYTE PTR DS:[ESI]
		INC ESI
label5:
		CMP ECX,0x40                                          //  Switch (cases 0..3F)
	JB label12
		MOV EDX,ECX                                         //  Default case of switch label5
		MOV EDI,EAX
		SHR EDX,0x02
		AND EDX,0x07
		SUB EDI,EDX
		XOR EDX,EDX
		MOV DL,BYTE PTR DS:[ESI]
		SHL EDX,0x03
		SUB EDI,EDX
		DEC EDI
		INC ESI
		SHR ECX,0x05
		DEC ECX
label25:
		MOV DL,BYTE PTR DS:[EDI]
		MOV BYTE PTR DS:[EAX],DL
		MOV DL,BYTE PTR DS:[EDI+0x01]
		INC EAX
		INC EDI
		MOV BYTE PTR DS:[EAX],DL
		INC EAX
		INC EDI
label13:
		MOV DL,BYTE PTR DS:[EDI]
		MOV BYTE PTR DS:[EAX],DL
		INC EAX
		INC EDI
		DEC ECX
	JNZ label13
	JMP label14
label12:
		CMP ECX,0x20
	JB label15
		AND ECX,0x1F                                          //  Cases 20,21,22,23,24,25,26,27,28,29,2A,2B,2C,2D,2E,2F,30,31,32,33,34,35,36,37,38,39,3A,3B,3C,3D,3E,3F of switch label5
	JNZ label16
		CMP BYTE PTR DS:[ESI],0
	JNZ label17
label18:
		MOV DL,BYTE PTR DS:[ESI+0x01]
		ADD ECX,0xFF
		INC ESI
		TEST DL,DL
	JE label18
label17:
		XOR EDX,EDX
		MOV DL,BYTE PTR DS:[ESI]
		INC ESI
		LEA ECX,DWORD PTR DS:[ECX+EDX+0x1F]
label16:
		XOR EDX,EDX
		MOV EDI,EAX
		MOV DX,WORD PTR DS:[ESI]
		SHR EDX,0x02
		SUB EDI,EDX
		DEC EDI
		ADD ESI,0x02
	JMP label19
label15:
		CMP ECX,0x10
	JB label20
		MOV EDX,ECX                                         //  Cases 10,11,12,13,14,15,16,17,18,19,1A,1B,1C,1D,1E,1F of switch label5
		MOV EDI,EAX
		AND EDX,0x08
		SHL EDX,0x0B
		SUB EDI,EDX
		AND ECX,0x07
	JNZ label21
		CMP BYTE PTR DS:[ESI],0x00
	JNZ label22
label23:
		MOV DL,BYTE PTR DS:[ESI+0x01]
		ADD ECX,0xFF
		INC ESI
		TEST DL,DL
	JE label23
label22:
		XOR EDX,EDX
		MOV DL,BYTE PTR DS:[ESI]
		INC ESI
		LEA ECX,DWORD PTR DS:[ECX+EDX+0x07]
label21:
		XOR EDX,EDX
		MOV DX,WORD PTR DS:[ESI]
		ADD ESI,0x02
		SHR EDX,0x02
		SUB EDI,EDX
		CMP EDI,EAX
	JE label24
		SUB EDI,0x4000
label19:
		CMP ECX,0x06
	JB label25
		MOV EDX,EAX
		SUB EDX,EDI
		CMP EDX,0x04
	JL label25
		MOV EDX,DWORD PTR DS:[EDI]
		ADD EDI,0x04
		MOV DWORD PTR DS:[EAX],EDX
		ADD EAX,0x04
		SUB ECX,0x02
label26:
		MOV EDX,DWORD PTR DS:[EDI]
		SUB ECX,0x04
		MOV DWORD PTR DS:[EAX],EDX
		ADD EAX,0x04
		ADD EDI,0x04
		CMP ECX,0x04
	JNB label26
		TEST ECX,ECX
	JBE label14
label27:
		MOV DL,BYTE PTR DS:[EDI]
		MOV BYTE PTR DS:[EAX],DL
		INC EAX
		INC EDI
		DEC ECX
	JNZ label27
	JMP label14
label20:
		SHR ECX,0x02                                           //  Cases 0,1,2,3,4,5,6,7,8,9,A,B,C,D,E,F of switch label5
		MOV EDX,EAX
		SUB EDX,ECX
		XOR ECX,ECX
		MOV CL,BYTE PTR DS:[ESI]
		SHL ECX,0x02
		SUB EDX,ECX
		DEC EDX
		INC ESI
	JMP lable28
label24:
		MOV ECX,DWORD PTR SS:[ESP+0x1C]
		SUB EAX,ECX
		CMP ESI,EBP
		MOV DWORD PTR DS:[EBX],EAX
	JNZ label29
		POP EDI
		POP ESI
		POP EBP
		XOR EAX,EAX
		POP EBX
		RETN
label29:
		SBB EAX,EAX
		POP EDI
		AND AL,0xFC
		POP ESI
		POP EBP
		ADD EAX, -4
		POP EBX
		RETN
	}
}


// Decompress wrapper function
extern void LZObject_Decompress(LPBYTE src, DWORD srcLen, LPBYTE dst, DWORD * ptrNewLen, void * workMemory)
{
	__asm
	{
		MOV EDI, src
		MOV EAX, dst
		push workMemory
		MOV EDX, ptrNewLen
		push EDX
		MOV EDX, srcLen
		PUSH EAX
		PUSH EDX
		ADD EDI, 4
		PUSH EDI
		call ASM_LZO_DECOMPRESS
		ADD ESP, 0x14
		TEST EAX, EAX
		JE LABEL1
		INT 3 // Error, don't continue
LABEL1:
		NOP
	}
}

#endif