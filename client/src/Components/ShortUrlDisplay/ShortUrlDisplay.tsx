import React from 'react';
import {Button, IconButton, Input, InputGroup, InputRightElement } from '@chakra-ui/react';
import { BiSolidCopy } from "react-icons/bi";

interface ShortUrlDisplayProps {
  shortenedUrl: string;
}

const ShortUrlDisplay: React.FC<ShortUrlDisplayProps> = ({ shortenedUrl }) => {

  const copyToClipboardOnClick = (text : string) => {

  }

  return (
    <InputGroup>
      <Input
        value={shortenedUrl}
        isReadOnly
        placeholder="Your shortened URL will appear here"
        size="lg"
        w="100%"
        variant="filled"
      />
      {shortenedUrl && (
        <InputRightElement>
          <IconButton
            onClick={() => {navigator.clipboard.writeText(shortenedUrl)}}
            aria-label='Copy URL'
            icon={<BiSolidCopy />}
            fontSize="1.5em"
            size="sm"
            mt="5px"
          />
        </InputRightElement>
      )}
    </InputGroup>
  );
};

export default ShortUrlDisplay;
