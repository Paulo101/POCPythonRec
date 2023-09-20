using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Python.Runtime;
using static System.Net.WebRequestMethods;

public static class PythonRecommendationService
{
    public static List<string> GetRecommendations(string accommodationName, IWebHostEnvironment env)
    {
        using (Py.GIL()) // Acquire the Python Global Interpreter Lock (GIL)
        {
            dynamic top_10 = null;

            try
            {
                // Import necessary Python modules
                dynamic pd = Py.Import("pandas");
                dynamic sklearn = Py.Import("sklearn");
                dynamic cosine_similarity = sklearn.metrics.pairwise.cosine_similarity;
                dynamic TfidfVectorizer = sklearn.feature_extraction.text.TfidfVectorizer;

                // Load accommodations data from a CSV file (replace with your actual data path)
                dynamic accommodations = pd.read_csv(Path.Combine(env.WebRootPath, "ccommodation.csv"));

                // Load addresses data from a CSV file (replace with your actual data path)
                dynamic addresses = pd.read_csv(Path.Combine(env.WebRootPath, "Address.csv"));

                // Merge accommodations and addresses data
                dynamic df = pd.merge(accommodations, addresses, "AddressId", "AddressId");

                // Combine relevant columns into a single text column for TF-IDF
                dynamic accommodations_combined = df["Price"] + " " +
                                                 df["rating"] + " " +
                                                 df["RoomTypeId"] + " " +
                                                 df["Surburb"];

                // Create a TfidfVectorizer object
                dynamic tfidf = TfidfVectorizer();
                dynamic tfidf_matrix = tfidf.fit_transform(accommodations_combined);

                // Calculate the cosine similarity matrix
                dynamic similarity_matrix = cosine_similarity(tfidf_matrix);

                // Find the index of the selected accommodation
                int accommodation_index = df[df["AccomodationName"] == accommodationName].index[0];

                // Get the top 10 most similar accommodations
                top_10 = df.iloc[accommodation_index].sort_values();
            }
            catch (Exception ex)
            {
                // Handle exceptions here
                Console.WriteLine($"An error occurred: {ex.Message}");
            }

            // Convert the top_10 result to a C# List<string>
            return top_10!.Select().ToList();
        }
    }
}
