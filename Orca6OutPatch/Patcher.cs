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
        const string IRString = @"VIBRATIONAL FREQUENCIES";
        const string MagnetString = @"DIPOLE MOMENT";

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
            string FP = new FileInfo(fileName).Directory.FullName;
            string FE = Path.GetExtension(fileName);
            patchedFileName = Path.Combine(FP, FN + $"_p{FE}");
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
                    if (line.Contains(IRString))
                        PatchIR(file, writer, ref n, ref patched);

                    if (line.Contains(MagnetString))
                        PatchMagnet(file, writer, ref n, ref patched);
                }

                Console.WriteLine(patched
                    ? $"Патч файла {patchedFileName} завершён"
                    : $"Патч файла {patchedFileName} не выполнен. Файл уже имеет правильную структуру.");
                return true;
            }

            return false;
        }

        private static void PatchIR(StreamReader file, StreamWriter writer, ref int n, ref bool patched)
        {
            string line;
            {
                // Пропустим 4 строки
                LeaveLines(4, file, writer, ref n);

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

        private static void PatchMagnet(StreamReader file, StreamWriter writer, ref int n, ref bool patched)
        {
            string line;
            {
                // Пропустим 2 строки
                LeaveLines(2, file, writer, ref n);

                // Уберём 7 строк
                DeleteLines(7, file,ref n);

                PatchMagnitudeLine(file, writer, ref n);
                PatchMagnitudeLine(file, writer, ref n);
                PatchMagnitudeLine(file, writer, ref n);
                LeaveLines(1, file, writer, ref n);
                PatchMagnitudeLine(file, writer, ref n);
                LeaveLines(1, file, writer, ref n);
                patched = true;
            }
        }

        /// <summary>
        /// Перемещает строки без изменений из одного файла в другой
        /// </summary>
        /// <param name="Lines">Сколько строк перенести</param>
        /// <param name="file">Откуда</param>
        /// <param name="writer">Куда</param>
        /// <param name="n">Счётчик строк</param>
        private static void LeaveLines(int Lines, StreamReader file, StreamWriter writer, ref int n)
        {
            string line;
            for (int i = 0; i < Lines; i++)
            {
                line = file.ReadLine();
                writer.WriteLine(line);
                n++;
            }
        }

        /// <summary>
        /// Патчит строку магнитуды 
        /// </summary>
        /// <param name="Lines">Сколько строк перенести</param>
        /// <param name="file">Откуда</param>
        /// /// <param name="writer">Куда</param>
        /// <param name="n">Счётчик строк</param>
        private static void PatchMagnitudeLine(StreamReader file, StreamWriter writer, ref int n)
        {
            string line = file.ReadLine();
            n++;
            line = line.Remove(37, 4);
            line = line.Remove(51, 4);
            if (line.Length > 66) line = line.Remove(66);

            writer.WriteLine(line);
        }

        /// <summary>
        /// Удаляет строки из файла
        /// </summary>
        /// <param name="file">Откуда</param>
        /// <param name="n">Счётчик строк</param>
        private static void DeleteLines(int Lines, StreamReader file, ref int n)
        {
            string line;
            for (int i = 0; i < Lines; i++)
            {
                line = file.ReadLine();
                n++;
            }
        }
    }
}
