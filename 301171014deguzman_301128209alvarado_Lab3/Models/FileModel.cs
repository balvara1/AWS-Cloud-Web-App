using Amazon.DynamoDBv2.DataModel;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace _301171014deguzman_301128209alvarado_Lab3.Models
{
    // table in DynamoDB is MovieCatalog
    [DynamoDBTable("MovieCatalog")]
    public class FileModel : Movie
    {
        [Required]
        [DynamoDBIgnore]
        public override IFormFile File { get; set; }
    }

}