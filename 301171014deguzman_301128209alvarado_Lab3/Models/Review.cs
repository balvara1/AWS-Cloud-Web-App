using Amazon.DynamoDBv2.DataModel;
using System;

namespace _301171014deguzman_301128209alvarado_Lab3.Models
{
    // table in DynamoDB is MovieCatalog
    // [DynamoDBTable("MovieCatalog")]
    // public class Review : Movie

    public partial class Review 
    {
        //public long MovieId { get; set; }
        //public override String CommentId { get; set; }
        //public override string ReviewerId { get; set; }
        //public override int Rating { get; set; }
        //public override string Comment { get; set; }
        //public override long CommentTime { get; set; }

        public string CommentId { get; set; }
        public string ReviewerId { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
        public long CommentTime { get; set; }

        public override string ToString()
        {
            return $"\nCommentId: {CommentId}\nReviewerId: {ReviewerId}\nRating:{Rating}\nComment:{Comment}\nCommentTime:{CommentTime}";
        }
    }
}