import React from 'react';
import { Input } from '@chakra-ui/react';

interface ShortUrlDisplayProps {
  shortenedUrl: string;
}

const ShortUrlDisplay: React.FC<ShortUrlDisplayProps> = ({ shortenedUrl }) => {
  return (
    <Input
      value={shortenedUrl}
      isReadOnly
      placeholder="Your shortened URL will appear here"
      size="lg"
      w="100%"
      variant="filled"
    />
  );
};

export default ShortUrlDisplay;
