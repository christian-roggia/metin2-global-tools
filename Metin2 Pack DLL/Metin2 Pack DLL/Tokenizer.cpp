#ifndef __TOKENIZER__
#define __TOKENIZER__

#include "stdafx.h"
#include "Tokenizer.h"

// Tokenizes a string into a vector
std::vector<std::string> TokenizeString(const std::string& str, const std::string& delim)
{
	using namespace std;
	vector<string> tokens;
	size_t p0 = 0, p1 = string::npos;
	while(p0 != string::npos)
	{
		p1 = str.find_first_of(delim, p0);
		if(p1 != p0)
		{
			string token = str.substr(p0, p1 - p0);
			tokens.push_back(token);
		}
		p0 = str.find_first_not_of(delim, p1);
	}
	return tokens;
}

#endif