import React from 'react';
import UrlShortener from "../UrlShortener/UrlShortener";
import {MAX_URL_LENGTH, MAX_ALIAS_LENGTH} from "../../Constants/Lengths";

const UrlShortenerMainComponent : React.FC = () => {
    
    const heading = "ShortTake";
    const tagline = "Shorten a long URL. Like a director's cut. But with links!";


    const URL_REGEX = /^(?:(?:https?|http):\/\/)(?:\S+(?::\S*)?@)?(?:(?!(?:10|127)(?:\.\d{1,3}){3})(?!(?:169\.254|192\.168)(?:\.\d{1,3}){2})(?!172\.(?:1[6-9]|2\d|3[0-1])(?:\.\d{1,3}){2})(?:[1-9]\d?|1\d\d|2[01]\d|22[0-3])(?:\.(?:1?\d{1,2}|2[0-4]\d|25[0-5])){2}(?:\.(?:[1-9]\d?|1\d\d|2[0-4]\d|25[0-4]))|(?:(?:[a-z0-9\u00a1-\uffff][a-z0-9\u00a1-\uffff_-]{0,62})?[a-z0-9\u00a1-\uffff]\.)+(?:[a-z\u00a1-\uffff]{2,}\.?))(?::\d{2,5})?(?:[/?#]\S*)?$/;
    const ALIAS_REGEX = /^[a-zA-Z0-9]+$/;
    
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

        let isAlphanumeric : boolean = ALIAS_REGEX.test(customAlias);

        return isAlphanumeric;
    }
    
    return (<div className="urlShortenerMain">
        
        <UrlShortener
            maxUrlLength={MAX_URL_LENGTH}
            maxAliasLength={MAX_ALIAS_LENGTH}
            urlValidator={urlIsValid}
            aliasValidator={customAliasIsValid}
            componentHeading={heading}
            componentTagline={tagline}/>

    </div>);
}


export default UrlShortenerMainComponent; 