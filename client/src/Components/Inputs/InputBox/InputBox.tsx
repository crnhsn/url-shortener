import React from "react"; 
import {Input} from '@chakra-ui/react';

interface InputBoxProps {
    placeholderText : string,
    onChange : (event: React.ChangeEvent<HTMLInputElement>) => void,
    className : string
}
const InputBox: React.FC<InputBoxProps> = ({ placeholderText, onChange, className }) => {
    return (
        <div className={className}>
            <Input placeholder={placeholderText} onChange={onChange} />
        </div>
    );
};

export default InputBox;