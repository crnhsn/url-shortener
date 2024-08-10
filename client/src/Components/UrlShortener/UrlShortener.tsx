import React, {ChangeEvent, useState} from 'react';
import {Box, Button, Card, CardHeader, Center, Heading, Stack, VStack} from "@chakra-ui/react";

import InputBox from '../Inputs/InputBox/InputBox';

import {shortenUrl} from "../../API/UrlShortenerAPI";
import ShortUrlDisplayWithCopy from '../ShortUrlDisplay/ShortUrlDisplayWithCopy';


const UrlShortener : React.FC = () => {
    
    const [urlToShorten, setUrlToShorten] = useState("");
    const [customAlias, setCustomAlias] = useState("");
    const [shortenedUrl, setShortenedUrl] = useState("");
    
    // on initial load - url box is empty, so it's invalid; custom alias box is empty, which is valid
    // but we still both to true / valid, because otherwise initial load would show an error underneath
    // the url box
    const [urlToShortenValid, setUrlToShortenValid] = useState(true);
    const [customAliasValid, setCustomAliasValid] = useState(true);

    const [urlErrorMessage, setUrlErrorMessage] = useState("");
    const [aliasErrorMessage, setAliasErrorMessage] = useState("");

    const MAX_ALIAS_LENGTH = 20; // todo - lift this into env var somehow, should sync with server max length
    const ALPHANUMERIC_REGEX = /^[a-zA-Z0-9]+$/;

    const MAX_URL_LENGTH = 2048; // todo - lift this into env var somehow, should sync with server max length
    const URL_REGEX = /^(?:(?:https?|http):\/\/)(?:\S+(?::\S*)?@)?(?:(?!(?:10|127)(?:\.\d{1,3}){3})(?!(?:169\.254|192\.168)(?:\.\d{1,3}){2})(?!172\.(?:1[6-9]|2\d|3[0-1])(?:\.\d{1,3}){2})(?:[1-9]\d?|1\d\d|2[01]\d|22[0-3])(?:\.(?:1?\d{1,2}|2[0-4]\d|25[0-5])){2}(?:\.(?:[1-9]\d?|1\d\d|2[0-4]\d|25[0-4]))|(?:(?:[a-z0-9\u00a1-\uffff][a-z0-9\u00a1-\uffff_-]{0,62})?[a-z0-9\u00a1-\uffff]\.)+(?:[a-z\u00a1-\uffff]{2,}\.?))(?::\d{2,5})?(?:[/?#]\S*)?$/;



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

        if (urlInvalid)
        {
            setUrlToShortenValid(false);
            setUrlErrorMessage("Invalid URL. Please enter a valid URL, starting with http or https.");
        }
        else
        {
            setUrlToShortenValid(true);
            setUrlErrorMessage("");
        }

        if (aliasInvalid)
        {
            setCustomAliasValid(false);
            setAliasErrorMessage(`Custom alias must be alphanumeric and ${MAX_ALIAS_LENGTH} characters or fewer.`);
        }
        else {
            setCustomAliasValid(true);
            setAliasErrorMessage("");
        }

        if (urlInvalid || aliasInvalid) {
            setShortenedUrl(""); // an input failure should reset the URL display
            return; // don't move on and execute the request if either input is invalid
        }

        try {
            // if we get to this point, then the inputs are valid
            // so execute the request
            const response = await shortenUrl(urlToShorten, customAlias);
            setShortenedUrl(response);

        } catch (error) {
            // todo: handle errors
            console.error("An error occurred:", error);
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