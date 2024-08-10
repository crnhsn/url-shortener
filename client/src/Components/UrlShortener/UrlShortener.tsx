import React, {ChangeEvent, useState} from 'react';
import {Button} from "@chakra-ui/react";

import InputBox from '../Inputs/InputBox/InputBox';


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

    const handleSubmit = () => {
        console.log("called handle submit");
        sendShortenRequest();
    }


    const sendShortenRequest = () => {

        if (!urlIsValid(urlToShorten)) {

            // display error

        }

        if (!customAliasIsValid(customAlias)) {
            // display error
        }


        console.log("todo: implement this! actually send the request");

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
        <div className="UrlShortener">

            <InputBox placeholderText={"link to shorten"}
                      onChange={handleUrlToShortenChange}
                      onKeyDown={handleKeyDown}
                      className={"UrlInputBox"} />

            <InputBox placeholderText={"custom alias (optional)"}
                      onChange={handleCustomAliasChange}
                      onKeyDown={handleKeyDown}
                      className={"CustomAliasInputBox"} />

            <Button onClick={handleSubmitButtonClick}>
                Shorten link!
            </Button>

        </div>
    ); 
    
}


export default UrlShortener; 