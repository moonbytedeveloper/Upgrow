using System.IO;

namespace VerifyIndia.Application.Utilities
{
    public static class KeyPathResolver
    {
        private const string ApplicationProjectFolderName = "VerifyIndia.Application";
        private const string KeysFolderName = "Keys";

        public static string GetPrivateKeyPath() => ResolveKeyPath("private.pem");

        public static string GetPublicKeyPath() => ResolveKeyPath("public.pem");

        private static string ResolveKeyPath(string keyFileName)
        {
            var directCandidates = new[]
            {
                Path.Combine(AppContext.BaseDirectory, KeysFolderName, keyFileName),
                Path.Combine(Directory.GetCurrentDirectory(), KeysFolderName, keyFileName),
                Path.Combine(AppContext.BaseDirectory, keyFileName),
                Path.Combine(Directory.GetCurrentDirectory(), keyFileName)
            };

            foreach (var candidate in directCandidates)
            {
                if (File.Exists(candidate))
                {
                    return candidate;
                }
            }

            var startDirectories = new[]
            {
                Directory.GetCurrentDirectory(),
                AppContext.BaseDirectory
            };

            foreach (var start in startDirectories)
            {
                var resolved = TryResolveFrom(start, keyFileName);
                if (resolved is not null)
                {
                    return resolved;
                }
            }

            throw new InvalidOperationException(
                $"Unable to locate '{keyFileName}'. Checked runtime and source paths, including '{ApplicationProjectFolderName}/{KeysFolderName}'.");
        }

        private static string? TryResolveFrom(string? startDirectory, string keyFileName)
        {
            if (string.IsNullOrWhiteSpace(startDirectory) || !Directory.Exists(startDirectory))
            {
                return null;
            }

            var current = new DirectoryInfo(startDirectory);

            while (current is not null)
            {
                var fromSolutionRoot = Path.Combine(current.FullName, ApplicationProjectFolderName, KeysFolderName, keyFileName);
                if (File.Exists(fromSolutionRoot))
                {
                    return fromSolutionRoot;
                }

                if (string.Equals(current.Name, ApplicationProjectFolderName, StringComparison.OrdinalIgnoreCase))
                {
                    var fromApplicationRoot = Path.Combine(current.FullName, KeysFolderName, keyFileName);
                    if (File.Exists(fromApplicationRoot))
                    {
                        return fromApplicationRoot;
                    }
                }

                current = current.Parent;
            }

            return null;
        }

        public static async Task<string> GetPrivateKeyAsync()
        {
            var privateKeyPath = GetPrivateKeyPath();
            return await File.ReadAllTextAsync(privateKeyPath);
        }
        public static async Task<string> GetPublicKeyAsync()
        {
            var publicKeyPath = GetPublicKeyPath();
            return await File.ReadAllTextAsync(publicKeyPath);
        }
    }
}