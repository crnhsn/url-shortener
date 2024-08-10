import React from 'react';
import {Button, IconButton, Input, InputGroup, InputRightElement, useToast} from '@chakra-ui/react';
import { BiSolidCopy } from "react-icons/bi";

interface ShortUrlDisplayWithCopyProps {
  shortenedUrl: string;
}

const ShortUrlDisplayWithCopy: React.FC<ShortUrlDisplayWithCopyProps> = ({ shortenedUrl }) => {

  const toast = useToast();

  const handleCopyIconClick = () => {

    navigator.clipboard.writeText(shortenedUrl).then(() => {
      toast({
        title: "Copied!",
        status: "success",
        duration: 600,
        isClosable: true,
      });
    }).catch(() => {
      toast({
        title: "Sorry, copy failed! Please try copying manually.",
        status: "error",
        duration: 1000,
        isClosable: true,
      });

    });
  };

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
            onClick={handleCopyIconClick}
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

export default ShortUrlDisplayWithCopy;
