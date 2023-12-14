﻿namespace TradeHarborApi.Models.AuthDtos
{
    /// <summary>
    /// Represents the user profile information extracted from a JWT token.
    /// </summary>
    public class JwtUserProfileDto
    {
        /// <summary>
        /// Gets or sets the user ID.
        /// </summary>
        public string UserId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the first name of the user.
        /// </summary>
        public string FirstName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the last name of the user.
        /// </summary>
        public string LastName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the username of the user.
        /// </summary>
        public string Username { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the URL of the user's profile picture.
        /// </summary>
        public string ProfilePictureUrl { get; set; } = string.Empty;
    }
}
