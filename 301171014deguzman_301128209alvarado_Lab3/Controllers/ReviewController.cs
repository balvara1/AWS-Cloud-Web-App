

using _301171014deguzman_301128209alvarado_Lab3.Models;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.Model;
using Amazon.S3;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace _301171014deguzman_301128209alvarado_Lab3
{
    public class ReviewController : Controller
    {

        private readonly IAmazonDynamoDB _dynamoDBClient;
        private readonly IDynamoDBContext _dynamoDBContext;
        private static IAmazonS3 _s3Client;


        public ReviewController(IAmazonDynamoDB dynamoDBClient, IDynamoDBContext dynamoDBContext, IAmazonS3 s3Client)
        {
            _dynamoDBClient = dynamoDBClient;
            _dynamoDBContext = dynamoDBContext;
            _s3Client = s3Client;
        }

        public async Task<IActionResult> Create(string movieId)
        {
            if (movieId == null)
            {
                return NotFound();
            }

            try
            {
                long parsedMovieId = Int64.Parse(movieId);
                Movie movie = await GetMovie(parsedMovieId);

                if (movie == default)
                {
                    return NotFound();
                }

                @ViewBag.MovieTitle = movie.Title;
                @ViewBag.MovieId = movieId;
                return View();
            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Review review, string MovieId)
        {
            // create movie item
            Console.WriteLine($"Movie Id: {MovieId}");
            Console.WriteLine($"Comment: {review.Comment}");

            // get the movie
            try
            {
                long parsedMovieId = Int64.Parse(MovieId);
                // get the movie object
                Movie movie = await GetMovie(parsedMovieId);

                if (movie == default)
                {
                    return NotFound();
                }

                if (movie.Reviews == null)
                {
                    movie.Reviews = new List<Review>();
                
                // generate unique comment id so we can locate the review easily if a user wants to edit it
                review.CommentId = Guid.NewGuid().ToString();
                review.CommentTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                var loggedInUser = HttpContext.Session.GetString("LoggedInUser");
                review.ReviewerId = loggedInUser;             

                movie.Reviews.Add(review);

                // update overall rating
                movie.OverallRating = computeOverallRating(movie);

                if (ModelState.IsValid)
                {
                    await _dynamoDBContext.SaveAsync<Movie>(movie);
                }
                }
                else
                {
                    // generate unique comment id so we can locate the review easily if a user wants to edit it
                    review.CommentId = Guid.NewGuid().ToString();
                    review.CommentTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                    var loggedInUser = HttpContext.Session.GetString("LoggedInUser");
                    review.ReviewerId = loggedInUser;

                    movie.Reviews.Add(review);

                    // update overall rating
                    movie.OverallRating = computeOverallRating(movie);

                    if (ModelState.IsValid)
                    {
                        await _dynamoDBContext.SaveAsync<Movie>(movie);
                    }
                }
                return RedirectToAction("Details", "Movies", new { movieId = MovieId });
            }
            catch (Exception ex)
            {
                return NotFound();
            }            
        }
        // GET: Review/Update?movieId=<MovieID>
        public async Task<IActionResult> Update(string commentId, string movieId)
        {
            if (movieId == null)
            {
                return NotFound();
            }

            try
            {
                long parsedMovieId = Int64.Parse(movieId);
                Movie movie = await GetMovie(parsedMovieId);

                if (movie == default)
                {
                    return NotFound();
                }

                // get the comment item
                Review reviewForEdit = movie.Reviews.First(aReview => aReview.CommentId.Equals(commentId));

                @ViewBag.MovieTitle = movie.Title;
                @ViewBag.MovieId = movieId;
                
                return View(reviewForEdit);
            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }


        // POST: Review/Update?movieId=<MovieID>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(Review review, string MovieId, string CommentId)
        {
            // get the movie
            try
            {
                long parsedMovieId = Int64.Parse(MovieId);
                // get the movie object
                Movie movie = await GetMovie(parsedMovieId);
                

                if (movie == default)
                {
                    return NotFound();
                }

                // get the comment item to be updated
                Review reviewForEdit = movie.Reviews.First(aReview => aReview.CommentId.Equals(CommentId));

                // update the comment, rating and time
                reviewForEdit.Rating = review.Rating;
                reviewForEdit.Comment = review.Comment;
                reviewForEdit.CommentTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

                // update overall rating
                movie.OverallRating = computeOverallRating(movie);

                if (ModelState.IsValid)
                {
                    await _dynamoDBContext.SaveAsync<Movie>(movie);
                }

                return RedirectToAction("Details", "Movies", new { movieId = MovieId });
            }

            catch (Exception ex)
            {
                return NotFound();
            }
        }
        
        private double computeOverallRating(Movie movie)
        {
            double overllRating = 0.0;

            // round off to two decimal places
            overllRating = Math.Round(movie.Reviews.Average(review => review.Rating), 2);
            return overllRating;
        }

        private async Task<Movie> GetMovie(long movieId)
        {
            Movie selectedMovie = default;
            try
            {
                Movie selMovie = await _dynamoDBContext.LoadAsync<Movie>(movieId);
                if (selMovie != null)
                {
                    selectedMovie = selMovie;
                    
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"exception thrown: {ex.StackTrace}");
            }

            return selectedMovie;
        }
    }
}