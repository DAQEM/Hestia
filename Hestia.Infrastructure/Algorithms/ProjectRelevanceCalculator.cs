using Hestia.Domain.Models;

namespace Hestia.Infrastructure.Algorithms;

public class ProjectRelevanceCalculator
{
    public static double CalculateRelevanceScore(Project project, string searchTerm)
        {
            // Simplified relevance calculation based on various factors
            double nameSimilarityScore = CalculateSimilarityScore(project.Name, searchTerm);
            double summarySimilarityScore = CalculateSimilarityScore(project.Summary, searchTerm);
            double descriptionSimilarityScore = CalculateSimilarityScore(project.Description, searchTerm);
            double popularityScore = CalculatePopularityScore(project.Downloads);
            double featureScore = project.IsFeatured ? 1 : 0;
            double publishedScore = project.IsPublished ? 1 : 0;

            // You can adjust the weights of these factors based on their importance
            double relevanceScore = nameSimilarityScore * 0.6 +
                                    summarySimilarityScore * 0.4 +
                                    descriptionSimilarityScore * 0.3 +
                                    popularityScore * 0.2 +
                                    featureScore * 0.1 +
                                    publishedScore * 0.1;

            Console.WriteLine(project.Name + " " + relevanceScore);
            
            return relevanceScore;
        }

        public static double CalculateRelevanceScoreWithoutSearchTerm(Project project)
        {
            // Simplified relevance calculation based on various factors
            double popularityScore = CalculatePopularityScore(project.Downloads);
            double featureScore = project.IsFeatured ? 1 : 0;
            double publishedScore = project.IsPublished ? 1 : 0;

            // You can adjust the weights of these factors based on their importance
            double relevanceScore = popularityScore * 0.6 +
                                    featureScore * 0.2 +
                                    publishedScore * 0.2;

            Console.WriteLine(project.Name + " " + relevanceScore);
            
            return relevanceScore;
        }

        private static double CalculateSimilarityScore(string field, string searchTerm)
        {
            // Calculate Jaccard similarity coefficient
            if (string.IsNullOrEmpty(field) || string.IsNullOrEmpty(searchTerm))
                return 0;

            string[] fieldWords = field.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            string[] searchTermWords = searchTerm.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            // Convert both arrays to HashSet for faster intersection and union operations
            HashSet<string> fieldSet = [..fieldWords];
            HashSet<string> searchTermSet = [..searchTermWords];

            int intersectionCount = fieldSet.Intersect(searchTermSet).Count();
            int unionCount = fieldSet.Union(searchTermSet).Count();

            double jaccardSimilarity = (double)intersectionCount / unionCount;
            return jaccardSimilarity;
        }

        private static double CalculatePopularityScore(long downloads)
        {
            //Normalize downloads to a score between 0 and 1
            const long maxDownloads = 1000000; // Example: Assume a maximum of 1 million downloads
            return Math.Min(1, (double)downloads / maxDownloads);
        }
}