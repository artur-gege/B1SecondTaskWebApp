using B1SecondTaskWebAPI.Data;
using B1SecondTaskWebAPI.Models;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace B1SecondTaskWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public FileController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("Не выбран файл.");
            }

            using (var ms = new MemoryStream())
            {
                await file.CopyToAsync(ms);
                var workbook = new XLWorkbook(ms);
                var worksheet = workbook.Worksheet(1); // Доступ к первому листу

                // Находим строку "БАЛАНС" (последнюю строку с данными)
                var balanceRow = worksheet.Rows().Last();

                // Находим первую строку с данными (после пропуска заголовков)s
                var firstDataRow = worksheet.Rows()
                    .Skip(8) // Пропускаем первые 8 строк (заголовки)
                    .Where(row => !string.IsNullOrEmpty((string)row.Cell("A").Value)
                                  && ((string)row.Cell("A").Value).ToString() != "Б/сч") // Проверка на пустоту и "Б/сч"
                    .FirstOrDefault();

                // Если строка "БАЛАНС" найдена
                if (balanceRow != null && firstDataRow != null)
                {
                    // Создание записи о загруженном файле в базе данных
                    var fileModel = new FileModel { FileName = file.FileName, UploadedDate = DateTime.Now };
                    _context.Files.Add(fileModel);
                    await _context.SaveChangesAsync(); // Сохраняем FileModel

                    // Получаем только что добавленный FileId
                    var fileId = fileModel.Id;

                    // Сохраняем данные из excel в БД
                    await SaveDataFromExcel(worksheet, firstDataRow, balanceRow, fileId);

                    return Ok($"Файл {file.FileName} успешно загружен.");
                }
                else
                {
                    return BadRequest("Файл не содержит необходимых данных.");
                }
            }
        }

        private async Task SaveDataFromExcel(IXLWorksheet worksheet, IXLRow firstDataRow, IXLRow balanceRow, int fileId)
        {
            string currentClass = null;

            for (int rowIndex = firstDataRow.RowNumber(); rowIndex <= balanceRow.RowNumber() - 1; rowIndex++)
            {
                var row = worksheet.Row(rowIndex);

                // Проверяем, является ли строка строкой класса
                if (row.Cell("A").Value.ToString()?.StartsWith("КЛАСС") == true)
                {
                    currentClass = row.Cell("A").Value.ToString();
                    continue;
                }

                // Чтение данных из строки
               

                var accountValue = row.Cell("A").Value.ToString();
                var activeDecimal = row.Cell("B").Value.ToString();
                var passiveDecimal = row.Cell("C").Value.ToString();
                var debitDecimal = row.Cell("D").Value.ToString();
                var creditDecimal = row.Cell("E").Value.ToString();
                var activeDecimal2 = row.Cell("F").Value.ToString();
                var passiveDecimal2 = row.Cell("G").Value.ToString();


                //var latestFile = _context.Files.OrderByDescending(f => f.Id).FirstOrDefault();
                // Создание новой записи DataModel
                var dataModel = new DataModel
                {
                    //FileId = latestFile != null ? latestFile.Id : 0,
                    FileId = fileId,
                    Class = currentClass,
                    Account = accountValue,
                    ActiveDecimal = decimal.TryParse(activeDecimal, out decimal active) ? active : 0,
                    PassiveDecimal = decimal.TryParse(passiveDecimal, out decimal passive) ? passive : 0,
                    DebitDecimal = decimal.TryParse(debitDecimal, out decimal debit) ? debit : 0,
                    CreditDecimal = decimal.TryParse(creditDecimal, out decimal credit) ? credit : 0,
                    ActiveDecimal2 = decimal.TryParse(activeDecimal2, out decimal active2) ? active2 : 0,
                    PassiveDecimal2 = decimal.TryParse(passiveDecimal2, out decimal passive2) ? passive2 : 0,
                };

                // Добавление записи в базу данных
                _context.FilesData.Add(dataModel);
            }

            // Сохранение изменений в БД
            await _context.SaveChangesAsync();
        }

        [HttpGet]
        public async Task<ActionResult<List<FileModel>>> GetFiles()
        {
            var files = await _context.Files.ToListAsync();
            return Ok(files);
        }

        [HttpGet("{fileId}")]
        public async Task<ActionResult<List<DataModel>>> GetDataByFileId(int fileId)
        {
            var dataFromFile = await _context.FilesData
                .Where(d => d.FileId == fileId)
                .ToListAsync();

            return Ok(dataFromFile);
        }
    }
}