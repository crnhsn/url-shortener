import React, {ChangeEvent, useState, useEffect} from 'react';
import {Box, Button, Card, Center, Heading, VStack} from "@chakra-ui/react";

import InputBox from '../Inputs/InputBox/InputBox';

import {shortenUrl} from "../../API/UrlShortenerAPI";
import ShortUrlDisplayWithCopy from '../ShortUrlDisplay/ShortUrlDisplayWithCopy';

import LottiePlayer from 'react-lottie-player';
import {AnimationUrl} from "../../Constants/Animation";

import {
    URL_NOT_PROVIDED_OR_INVALID_MESSAGE,
    URL_TOO_LONG_MESSAGE,
    CUSTOM_ALIAS_NOT_ALPHANUMERIC_OR_TOO_LONG_MESSAGE,
    CUSTOM_ALIAS_UNAVAILABLE_MESSAGE,
    UNEXPECTED_ERROR_MESSAGE
  } from '../../ErrorMessages/ClientErrorMessages';

import {
    URL_NOT_PROVIDED,
    URL_INVALID,
    URL_TOO_LONG,
    CUSTOM_ALIAS_NOT_ALPHANUMERIC,
    CUSTOM_ALIAS_TOO_LONG,
    CUSTOM_ALIAS_UNAVAILABLE,
    UNEXPECTED_ERROR
  } from '../../ErrorMessages/ServerErrorMessages';


interface UrlShortenerProps {
    maxUrlLength : number,
    maxAliasLength : number,
    urlValidator : (url: string) => boolean,
    aliasValidator : (alias: string) => boolean,
    componentHeading? : string,
    componentTagline? : string
}

const UrlShortener : React.FC<UrlShortenerProps> = ({urlValidator, aliasValidator, componentHeading, componentTagline}) => {

    const [urlToShorten, setUrlToShorten] = useState("");
    const [customAlias, setCustomAlias] = useState("");
    const [shortenedUrl, setShortenedUrl] = useState("");

    
    const [urlToShortenValid, setUrlToShortenValid] = useState(true);
    const [customAliasValid, setCustomAliasValid] = useState(true);

    const [urlErrorMessage, setUrlErrorMessage] = useState("");
    const [aliasErrorMessage, setAliasErrorMessage] = useState("");

    const [animationData, setAnimationData] = useState(null);
    const [animationKey, setAnimationKey] = useState(0); // will use this to re-render the animation as needed

    const urlIsValid = urlValidator;
    const aliasIsValid = aliasValidator;

    const animationUrl = AnimationUrl;

    useEffect(() => {
        // get animation data on initial load
        fetch(animationUrl)
            .then((response) => response.json())
            .then((data) => setAnimationData(data))
            .catch((error) => console.error("Error loading animation:", error));
    }, []);

    // every time a new shortened url is generated, increment the animation key
    // which will re-render the animation, since the animation key is on the animation component
    useEffect(() => {
        if (shortenedUrl) {
            setAnimationKey(animationKey + 1);
        }
    }, [shortenedUrl]);

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
        const aliasInvalid = !aliasIsValid(customAlias);

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
            return; // don't move on - we don't want to execute the request if either input is invalid
        }

        try {
            // if we are at this point
            // the inputs are valid
            // so reset the states that track validity
            // and execute api call
            setUrlErrorMessage("");
            setAliasErrorMessage("");
            setCustomAliasValid(true);
            setUrlToShortenValid(true);

            // API call here has following potential result states:
            // Success (200 ok)
            // Got to server, but server failed the request for some reason (server validation failed, duplicate short code, etc.)
            // Failed for some other reason - network failure, client side validation issue, etc.

            // The first two are states are encapsulated in the response object,
            // so we'd extract the error info from response and handle accordingly
            // Third case is thrown to this level and handled separately

            const response = await shortenUrl(urlToShorten, customAlias);

            // first case: request succeeded
            if (response.status === 200)
            {
                setShortenedUrl(response.data);
            }
            else // second case: request got to server, but server failed the request - so extract error info and handle
            {
                handleServerError(response.data);
            }
        }
        catch (error) // third case - failure for some other reason
        {
            console.log(error);
            setUrlErrorMessage(UNEXPECTED_ERROR_MESSAGE);
        }
    };

    const handleServerError = (errorMessage: string) => {

        setShortenedUrl(""); // an error should reset the URL display

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

    return (
        <Center height="100vh" bg="gray.50">
          <Box className="UrlShortener" p={4} w="lg"  bg="white" borderRadius="lg" boxShadow="lg">
            <VStack spacing={4}>
              <Card w="100%" borderRadius="sm" boxShadow="sm">
                <VStack spacing={4} p={4}>

                    {animationData && (
                                <LottiePlayer
                                    key={animationKey}
                                    animationData={animationData}
                                    play={true}
                                    style={{ height: 300, width: 300 }}
                                    loop={false}
                                />
                            )}

                    <Heading size="lg" textAlign="center" color="black" marginTop={animationData ? -12: 0}>
                      {componentHeading ? componentHeading : "URL Shortener"}
                      {componentTagline && (
                          <p style={{ fontSize: '0.8rem', color: 'gray', textAlign: 'center', marginTop: '0.5rem' }}>
                            {componentTagline}
                          </p>
                        )}
                    </Heading>


                  <InputBox
                    placeholderText="Link to shorten"
                    onChange={handleUrlToShortenChange}
                    onKeyDown={handleKeyDown}
                    className="UrlInputBox"
                    isInvalid={!urlToShortenValid}
                    errorMessage={urlErrorMessage}
                  />

                  <InputBox
                    placeholderText="Custom alias (optional)"
                    onChange={handleCustomAliasChange}
                    onKeyDown={handleKeyDown}
                    className="CustomAliasInputBox"
                    isInvalid={!customAliasValid}
                    errorMessage={aliasErrorMessage}
                  />

                  <Button
                    onClick={handleSubmitButtonClick}
                    size="md"
                    colorScheme="teal"
                    variant="solid"
                    alignSelf="center"
                  >
                    Shorten Link
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