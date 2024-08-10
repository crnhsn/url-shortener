import React, {ChangeEvent, useState} from 'react';
import {Box, Button, Card, CardHeader, Center, Heading, Stack, VStack} from "@chakra-ui/react";

import InputBox from '../Inputs/InputBox/InputBox';

import {shortenUrl} from "../../API/UrlShortenerAPI";
import ShortUrlDisplayWithCopy from '../ShortUrlDisplay/ShortUrlDisplayWithCopy';


const UrlShortener : React.FC = () => {

    const [urlToShorten, setUrlToShorten] = useState("");
    const [customAlias, setCustomAlias] = useState("");
    const [shortenedUrl, setShortenedUrl] = useState("");

    
    const [urlToShortenValid, setUrlToShortenValid] = useState(true);
    const [customAliasValid, setCustomAliasValid] = useState(true);

    const [urlErrorMessage, setUrlErrorMessage] = useState("");
    const [aliasErrorMessage, setAliasErrorMessage] = useState("");

    // todo - put all these constants into a file and sync them with server somehow
    // maybe a json config file that both the JS and C# read?
    const MAX_ALIAS_LENGTH = 8; // todo - lift this into env var somehow, should sync with server max length
    const ALPHANUMERIC_REGEX = /^[a-zA-Z0-9]+$/;

    const MAX_URL_LENGTH = 2048; // todo - lift this into env var somehow, should sync with server max length
    const URL_REGEX = /^(?:(?:https?|http):\/\/)(?:\S+(?::\S*)?@)?(?:(?!(?:10|127)(?:\.\d{1,3}){3})(?!(?:169\.254|192\.168)(?:\.\d{1,3}){2})(?!172\.(?:1[6-9]|2\d|3[0-1])(?:\.\d{1,3}){2})(?:[1-9]\d?|1\d\d|2[01]\d|22[0-3])(?:\.(?:1?\d{1,2}|2[0-4]\d|25[0-5])){2}(?:\.(?:[1-9]\d?|1\d\d|2[0-4]\d|25[0-4]))|(?:(?:[a-z0-9\u00a1-\uffff][a-z0-9\u00a1-\uffff_-]{0,62})?[a-z0-9\u00a1-\uffff]\.)+(?:[a-z\u00a1-\uffff]{2,}\.?))(?::\d{2,5})?(?:[/?#]\S*)?$/;

    // error messages the server sends back - todo - lift into file / env and sync with server
    const URL_NOT_PROVIDED = "URL_REQUIRED";
    const URL_INVALID = "URL_INVALID";
    const URL_TOO_LONG = "URL_LENGTH";

    const CUSTOM_ALIAS_NOT_ALPHANUMERIC = "CUSTOM_ALIAS_FORMAT";
    const CUSTOM_ALIAS_TOO_LONG = "CUSTOM_ALIAS_LENGTH";
    const CUSTOM_ALIAS_UNAVAILABLE = "CUSTOM_ALIAS_UNAVAILABLE";

    const UNEXPECTED_ERROR = "INTERNAL_SERVER_ERROR";

    // error messages client displays - todo - lift into file / env? maybe not env
    const URL_NOT_PROVIDED_OR_INVALID_MESSAGE = "Please enter a valid URL, starting with http or https.";
    const URL_TOO_LONG_MESSAGE = "The provided URL is too long. Please shorten the URL.";

    const CUSTOM_ALIAS_NOT_ALPHANUMERIC_OR_TOO_LONG_MESSAGE = `The custom alias must be alphanumeric and fewer than ${MAX_ALIAS_LENGTH} characters. Please choose another alias.`;
    const CUSTOM_ALIAS_UNAVAILABLE_MESSAGE = "The provided alias is not available. Please select another.";

    const UNEXPECTED_ERROR_MESSAGE = "An unexpected error occurred. Please try again later.";



    const handleUrlToShortenChange = (e : ChangeEvent<HTMLInputElement>) => {

        setUrlToShorten(e.target.value);
    }

    const handleCustomAliasChange = (e : ChangeEvent<HTMLInputElement>) => {

        setCustomAlias(e.target.value);
    }

    const handleSubmitButtonClick = () => {
        handleSubmit();
    }

    const handleKeyDown = (e: React.KeyboardEvent<HTMLInputElement>) => {
        if (e.key === 'Enter') {
            handleSubmit();
        }
    }

    const handleSubmit = async () => {
        const urlInvalid = !urlIsValid(urlToShorten);
        const aliasInvalid = !customAliasIsValid(customAlias);

        if (urlInvalid) {
            setUrlToShortenValid(false);
            setUrlErrorMessage(URL_NOT_PROVIDED_OR_INVALID_MESSAGE);
        }
        else
        {
            setUrlToShortenValid(true);
            setUrlErrorMessage("");
        }

        if (aliasInvalid)
        {
            setCustomAliasValid(false);
            setAliasErrorMessage(CUSTOM_ALIAS_NOT_ALPHANUMERIC_OR_TOO_LONG_MESSAGE);
        }
        else
        {
            setCustomAliasValid(true);
            setAliasErrorMessage("");
        }

        if (urlInvalid || aliasInvalid) {
            setShortenedUrl(""); // an input failure should reset the URL display
            return; // don't move on and execute the request if either input is invalid
        }

        try {
            // if we are at this point
            // the inputs are valid
            // so reset the state that tracks validity
            // and execute api call
            setUrlErrorMessage("");
            setAliasErrorMessage("");
            setCustomAliasValid(true);
            setUrlToShortenValid(true);

            // api call here has 3 potential states:
            // success (200 ok)
            // got to server, but server failed request (validation, etc.)
            // failed even before getting to server, or some other way
            // first two are encapsulated in the response
            // third one gets thrown up to this level
            const response = await shortenUrl(urlToShorten, customAlias);

            // this if / else handles the first two - if request succeeded
            // set the shortened url

            if (response.status === 200)
            {
                setShortenedUrl(response.data);
            }
                // if request got to server but failed, extract error info from request
                // and handle
            else
            {
                handleError(response.data);
            }
        }
        catch (error) // third case would get here
        {
            // todo - handle
        }
    };

    const handleError = (errorMessage: string) => {
        setShortenedUrl(""); // an input failure should reset the URL display
        switch (errorMessage)
        {
            case URL_NOT_PROVIDED:
                setUrlToShortenValid(false);
                setUrlErrorMessage(URL_NOT_PROVIDED_OR_INVALID_MESSAGE);
                break;

            case URL_INVALID:
                setUrlToShortenValid(false);
                setUrlErrorMessage(URL_NOT_PROVIDED_OR_INVALID_MESSAGE);
                break;

            case URL_TOO_LONG:
                setUrlToShortenValid(false);
                setUrlErrorMessage(URL_TOO_LONG_MESSAGE);
                break;

            case CUSTOM_ALIAS_NOT_ALPHANUMERIC:
                setCustomAliasValid(false);
                setAliasErrorMessage(CUSTOM_ALIAS_NOT_ALPHANUMERIC_OR_TOO_LONG_MESSAGE);
                break;

            case CUSTOM_ALIAS_TOO_LONG:
                setCustomAliasValid(false);
                setAliasErrorMessage(CUSTOM_ALIAS_NOT_ALPHANUMERIC_OR_TOO_LONG_MESSAGE);
                break;

            case CUSTOM_ALIAS_UNAVAILABLE:
                setCustomAliasValid(false);
                setAliasErrorMessage(CUSTOM_ALIAS_UNAVAILABLE_MESSAGE);
                break;

            case UNEXPECTED_ERROR:
                setUrlErrorMessage(UNEXPECTED_ERROR_MESSAGE);
                break;

            default:
                setUrlErrorMessage(UNEXPECTED_ERROR_MESSAGE);
                break;
        }
    };


    const urlIsValid = (url : string) : boolean => {
        if (url.length > MAX_URL_LENGTH) {
            return false;
        }

        return URL_REGEX.test(url);
    }

    const customAliasIsValid = (customAlias : string) : boolean => {

        if (!customAlias) {
            return true; // custom alias is optional, so if it's empty, it's valid
        }

        if (customAlias.length > MAX_ALIAS_LENGTH) {

            return false;

        }

        let isAlphanumeric : boolean = ALPHANUMERIC_REGEX.test(customAlias);

        return isAlphanumeric;
    }


    return (
        <Center height="100vh">
          <Box className="UrlShortener" p={6} maxW="md" w="100%">
            <VStack spacing={6}>
              <Card w="100%">
                <CardHeader>
                  <Heading size='md' textAlign="center">URL Shortener</Heading>
                </CardHeader>
                <VStack spacing={4} p={4}>

                  <InputBox
                    placeholderText="link to shorten"
                    onChange={handleUrlToShortenChange}
                    onKeyDown={handleKeyDown}
                    className="UrlInputBox"
                    isInvalid={!urlToShortenValid}
                    errorMessage={urlErrorMessage}
                  />

                  <InputBox
                    placeholderText="custom alias (optional)"
                    onChange={handleCustomAliasChange}
                    onKeyDown={handleKeyDown}
                    className="CustomAliasInputBox"
                    isInvalid={!customAliasValid}
                    errorMessage={aliasErrorMessage}
                  />

                  <Button
                    onClick={handleSubmitButtonClick}
                    size="md"
                    alignSelf="center"
                  >
                    Shorten link!
                  </Button>
                </VStack>
              </Card>
              <ShortUrlDisplayWithCopy shortenedUrl={shortenedUrl} />
            </VStack>
          </Box>
        </Center>
      );


    
}


export default UrlShortener; 