using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Demo.PL.Helpers
{
	public class DocumentSettings
	{
		public async static Task<string> UploadFileAsync(IFormFile file, string folderName)
		{
			var folderPath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\files", folderName);

			//var fileName = $"{Guid.NewGuid()}{Path.GetFileName(file.FileName)}";
			var fileName = $"{Guid.NewGuid()}{file.FileName}";

			var filePath = Path.Combine(folderPath, fileName);

			using var fileStream = new FileStream(filePath, FileMode.Create);

			await file.CopyToAsync(fileStream);

			return fileName;
		}

		public static void DeleteFile(string fileName , string folderName)
		{
			string filePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\files",folderName);
			if (File.Exists(filePath))
				File.Delete(filePath);

		}
	}
}
