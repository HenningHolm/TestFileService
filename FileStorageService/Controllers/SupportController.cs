using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Azure.Storage.Blobs.Models;
using FileStorageService.Data;
using FileStorageService.Models.Operations;
using FileStorageService.Models.Queries;
using FileStorageService.Models.RequestResponse;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace FileStorageService.Controllers
{
 
    public class SupportController : Controller
    {
        private readonly IDownloadFileOperation _downloadFileOperation;
        private readonly IDeleteFileOperation _deleteFileOperation;
        private readonly IGetFileListByPersonIdQuery _getFileListByPersonIdQuery;
        private readonly ILogger<SupportController> _logger;
        private readonly IUploadFileOperation _uploadFileOperation;
        private readonly IGenerateSharedAccessUriOperation _generateSharedAccessUriOperation;

        public SupportController(IDownloadFileOperation downloadFileOperation, IDeleteFileOperation deleteFileOperation, IUploadFileOperation uploadFileOperation,
            IGetFileListByPersonIdQuery getFileListByPersonIdQuery, ILogger<SupportController> logger, IGenerateSharedAccessUriOperation generateSharedAccessUriOperation)
        {
            _downloadFileOperation = downloadFileOperation;
            _getFileListByPersonIdQuery = getFileListByPersonIdQuery;
            _logger = logger;
            _generateSharedAccessUriOperation = generateSharedAccessUriOperation;
            _deleteFileOperation = deleteFileOperation;
            _uploadFileOperation = uploadFileOperation;

        }

        public IActionResult Index()
        {
            return View();
        }


        [HttpGet]
        public async Task<IActionResult> GetFiles(int userId, string type)
        {
            var files = await _getFileListByPersonIdQuery.ExecuteAsync(userId);
            ViewBag.Result = files.AsJson();
            return View("Index");
        }

        [HttpGet]
        public async Task<IActionResult> DownloadFile(Guid fileId)
        {
            var response = await _downloadFileOperation.ExecuteAsync(fileId);
            if (response == null)
            {
                ViewBag.Result = "Requested file not found";
                return View("Index");
            }
            return File(response.Content, "application/octet-stream", response.FileName);
        }


        [HttpPost]
        public async Task<IActionResult> UploadFile(int userId, string containerName, int accessLevel = 0)
        {
            var request = await HttpContext.Request.ReadFormAsync();
                if (request.Files == null)
                {
                    return BadRequest("Could not upload files");
                }

                if (request.Files.Count == 0)
                {
                    return BadRequest("Could not upload empty files");
                }

                var files = new List<FileUploadRequest>();
                foreach (var requestFile in request.Files)
                {
                    files.Add(new FileUploadRequest(requestFile.OpenReadStream(), requestFile.FileName));
                }

                //var containerName _logger.GetSourceSystem();
                //var userId  User.GetPersonId();
                try
                {
                    var result = await _uploadFileOperation.ExecuteAsync(files, userId, containerName, (PublicAccessType)accessLevel);
                    ViewBag.Result = result.AsJson();
                    return View("Index");
                }
                catch (Exception e)
                {
                    _logger.LogError("UploadFile Error " + e.Message, e);
                    ViewBag.Result = e.Message;
            }

            return View("Index");
        }

        [HttpGet]
        public async Task<IActionResult> DeleteFile(int personId, Guid fileId)
        {
            var response = await _deleteFileOperation.ExecuteAsync(fileId, personId);

            ViewBag.Result = response.AsJson();
            return View("Index");
        }

        [HttpGet]
        public async Task<IActionResult> GetSasUri(Guid fileId)
        {
            var url = await _generateSharedAccessUriOperation.ExecuteAsync(fileId);
            ViewBag.Result = url;
            return View("Index");
        }


    }

}