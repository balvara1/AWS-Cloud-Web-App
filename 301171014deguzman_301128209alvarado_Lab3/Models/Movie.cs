using Amazon.DynamoDBv2.DataModel;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace _301171014deguzman_301128209alvarado_Lab3.Models
{
    // table in DynamoDB is MovieCatalog
    [DynamoDBTable("MovieCatalog")]
    public partial class Movie
    {
        [DynamoDBHashKey]
        public long MovieID { get; set; }
        public string CreatorID { get; set; }
        public string Title { get; set; }
        public string Genre { get; set; }
        public int ReleaseYear { get; set; }
        public string Directors { get; set; }
        public string Cast { get; set; }
        public string MotionPictureRating { get; set; }
        public double OverallRating { get; set; }

        /************ Review Function ************/
        //public virtual String CommentId { get; set; }
        //public virtual string ReviewerId { get; set; }
        //public virtual int Rating { get; set; }
        //public virtual string Comment { get; set; }
        //public virtual long CommentTime { get; set; }

        public List<Review> Reviews { get; set; }

        /************ End of Review Function ************/

        /************ Upload Function ************/
        // For the naming of s3 object item key
        public string S3Key { get; set; }

        [DynamoDBIgnore]
        public virtual IFormFile File { get; set; }

        /************ End of Upload Function ************/



        public override string ToString()
        {
            return $"\nMovie ID: {MovieID}\nTitle: {Title}\nRelease Year: {ReleaseYear}\nGenre: {Genre}\nCast: {Cast}\nDirectors: {Directors}\nMotion Picture Rating: {MotionPictureRating}";
        }

        /* Add these ff for user comment records:
            ReviewerId
            Comment
            CommentTime
            Rating
        */

        /* Add these ff for the main movie record
         * CreatorId
         * OverallRating
         */

    }
}
