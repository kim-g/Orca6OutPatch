// OrcaPatcher.cpp: определяет точку входа для приложения.
//

#include "OrcaPatcher.h"

using namespace std;

int main(int argc, char* argv[])
{
    const bool DEBUG = true;
    if (argc == 1)
    {
        cout << "Укажите файл или директорию для конвертации" << endl;
        return 0;
    }
    
    for (int i=1; i < argc; ++i)
    {
        cout << argv[i] << endl;
    }
	return 0;
}
