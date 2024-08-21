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
            using (StreamReader file = new StreamReader(fileName))
            using (StreamWriter writer = new StreamWriter(patchedFileName, false))
            {
                int n = 0;
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
                            file.ReadLine();
                            writer.WriteLine("");
                            n++;
                        }

                        while ((line = file.ReadLine()) != "")
                        {
                            if (line == null) break;
                            n++;
                            line = line.Remove(0, 2);
                            line = line.Replace(":", ":  ");
                            line = line.Replace("^", "  ");
                            writer.WriteLine(line);
                            
                        }
                        
                    }
                }

                Console.WriteLine($"Патч файла {patchedFileName} завершён");
            }

            return false;
        }
    }
}
