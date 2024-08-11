import {MAX_ALIAS_LENGTH} from "../Constants/Lengths";

export const URL_NOT_PROVIDED_OR_INVALID_MESSAGE = "Please enter a valid URL, starting with http or https.";
export const URL_TOO_LONG_MESSAGE = "The provided URL is too long. Please shorten the URL.";
export const CUSTOM_ALIAS_NOT_ALPHANUMERIC_OR_TOO_LONG_MESSAGE = `The custom alias must be alphanumeric and fewer than ${MAX_ALIAS_LENGTH} characters. Please choose another alias.`;
export const CUSTOM_ALIAS_UNAVAILABLE_MESSAGE = "The provided alias is not available. Please select another.";
export const UNEXPECTED_ERROR_MESSAGE = "An unexpected error occurred. Please try again later.";