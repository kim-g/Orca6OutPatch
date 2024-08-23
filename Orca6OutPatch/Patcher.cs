using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orca6OutPatch
{
    public class Patcher
    {

        protected string fileName;
        protected string patchedFileName;
        const string SearchString = @"VIBRATIONAL FREQUENCIES";

        /// <summary>
        /// Класс обработки файла
        /// </summary>
        /// <param name="FileName">Имя файла</param>
        public Patcher(string FileName) 
        {
            if (!File.Exists(FileName))
            {
                Console.WriteLine($"Ошибка 001: Файл «{FileName}» не существует! Патч невозможен.");
                return;
            }

            fileName = FileName.Trim();
            string FN = Path.GetFileNameWithoutExtension(fileName);
            patchedFileName = FN + "_p.out";
        }

        /// <summary>
        /// Метод обработки файла. 
        /// </summary>
        /// <returns></returns>
        public bool Patch()
        {
            if (fileName == null) return false;
            
            using (StreamReader file = new StreamReader(fileName))
            using (StreamWriter writer = new StreamWriter(patchedFileName, false))
            {
                int n = 0;
                bool patched = true;
                string line;
                while ((line = file.ReadLine()) != null)
                {
                    n++;
                    writer.WriteLine(line);
                    if (line.Contains(SearchString))
                    {
                        // Пропустим 4 строки
                        for (int i = 0; i < 4; i++)
                        {
                            line = file.ReadLine();
                            writer.WriteLine(line);
                            n++;
                        }

                        while ((line = file.ReadLine()) != "")
                        {
                            if (line == null) break;
                            n++;

                            if (line[4] == ':')
                            {
                                patched = false;
                                writer.WriteLine(line);
                                continue;
                            }

                            line = line.Remove(0, 2);
                            line = line.Replace(":", ":  ");
                            writer.WriteLine(line);
                            
                        }
                        
                    }
                }

                Console.WriteLine(patched 
                    ? $"Патч файла {patchedFileName} завершён"
                    : $"Патч файла {patchedFileName} не выполнен. Файл уже имеет правильную структуру.");
                return true;
            }

            return false;
        }
    }
}
