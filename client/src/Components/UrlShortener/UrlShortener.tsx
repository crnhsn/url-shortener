import React, {ChangeEvent, useState} from 'react';
import {Button, Card, CardHeader, Center, Heading, Stack, VStack} from "@chakra-ui/react";

import InputBox from '../Inputs/InputBox/InputBox';

import axios from 'axios';
import {shortenUrl} from "../../API/UrlShortenerAPI";


const UrlShortener : React.FC = () => {
    
    const [urlToShorten, setUrlToShorten] = useState("");
    const [customAlias, setCustomAlias] = useState("");
    
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
        console.log("called handle submit");
        if (!urlIsValid(urlToShorten)) {

            // display error
            return; // exit function, todo 
        }

        if (!customAliasIsValid(customAlias)) {
            // display error
            return; // exit function, todo
        }

        try {

            await shortenUrl(urlToShorten, customAlias);

        }
        catch (error) {
            // todo: fill this out
        }
    }

    const urlIsValid = (longUrl : string) : boolean => {
        return true;
        // implement
    }

    const customAliasIsValid = (customAlias : string) : boolean => {
        return true;
        // implement
    }



    // todo:
    // attach keypress event listeners to both input text boxes, to invoke submitRequest
    // clicking on button calls onSubmitButtonClick, which calls submitRequest
    // submitRequest calls validateInput on both boxes
    // it then sends POST request to backend
    // errors are caught and processed accordingly by some kind of error handler


    return (
        <Center>
            <div className="UrlShortener">

                <VStack>
                    <Card>
                        <CardHeader>
                            <Heading size='md'>URL Shortener</Heading>
                        </CardHeader>
                            <VStack>

                                <InputBox placeholderText={"link to shorten"}
                                          onChange={handleUrlToShortenChange}
                                          onKeyDown={handleKeyDown}
                                          className={"UrlInputBox"} />

                                <InputBox placeholderText={"custom alias (optional)"}
                                          onChange={handleCustomAliasChange}
                                          onKeyDown={handleKeyDown}
                                          className={"CustomAliasInputBox"} />

                            </VStack>
                    </Card>

                    <Button onClick={handleSubmitButtonClick}>
                        Shorten link!
                    </Button>

                </VStack>

            </div>
        </Center>
    ); 
    
}


export default UrlShortener; 