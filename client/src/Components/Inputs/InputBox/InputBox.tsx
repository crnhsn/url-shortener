import React from "react"; 
import {FormControl, FormErrorMessage, Input} from '@chakra-ui/react';

interface InputBoxProps {
    placeholderText : string,
    onChange : (e: React.ChangeEvent<HTMLInputElement>) => void,
    onKeyDown : (e: React.KeyboardEvent<HTMLInputElement>) => void,
    className : string,
    isInvalid? : boolean,
    errorMessage? : string
}
const InputBox: React.FC<InputBoxProps> = ({ placeholderText, onChange, onKeyDown, className, isInvalid, errorMessage }) => {
    return (
        <div className={className}>
            <FormControl isInvalid={isInvalid}>
                <Input placeholder={placeholderText}
                       onChange={onChange}
                       onKeyDown={onKeyDown}
                       isInvalid={isInvalid}
                       borderColor={isInvalid ? "red.500" : "gray.200"}
                />
                <FormErrorMessage>{errorMessage}</FormErrorMessage>
            </FormControl>

        </div>
    );
};

export default InputBox;