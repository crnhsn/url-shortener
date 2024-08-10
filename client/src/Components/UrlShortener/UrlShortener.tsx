import React, {ChangeEvent, useState} from 'react';
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
    
 
    
    return (
        <div className="UrlShortener">
            <InputBox placeholderText={"link to shorten"} onChange={handleUrlToShortenChange} className={"UrlInputBox"} />
            <InputBox placeholderText={"custom alias (optional)"} onChange={handleCustomAliasChange} className={"CustomAliasInputBox"} />
        </div>
    ); 
    
}


export default UrlShortener; 