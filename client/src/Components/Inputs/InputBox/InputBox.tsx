import React from "react"; 
import {Input} from '@chakra-ui/react';

interface InputBoxProps {
    placeholderText : string,
    onChange : (e: React.ChangeEvent<HTMLInputElement>) => void,
    onKeyDown : (e: React.KeyboardEvent<HTMLInputElement>) => void,
    className : string
}
const InputBox: React.FC<InputBoxProps> = ({ placeholderText, onChange, onKeyDown, className }) => {
    return (
        <div className={className}>
            <Input placeholder={placeholderText} onChange={onChange} onKeyDown={onKeyDown} />
        </div>
    );
};

export default InputBox;