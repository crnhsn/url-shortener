import axios, { AxiosResponse } from "axios";

const BASE_URL = process.env.REACT_APP_API_BASE_URL; 

// validateStatus controls whether axios treats non-success status codes as errors
// by default axios throws error if non-success status code, and the caller
// loses easy access to the response object
// don't want this behavior here because non-success status codes
// should get handled by our client / frontend view depending on what the reason for the non-success is
// so the caller should still have response object access to handle the error appropriately
const api = axios.create({ baseURL: BASE_URL,
                           validateStatus: () => true});

export const shortenUrl = async (urlToShorten: string, customAlias?: string): Promise<AxiosResponse> => {
    try
    {
        const response = await api.post('/shorten', {
            longUrl: urlToShorten,
            customAlias: customAlias || undefined
        });
        return response;
    }

    catch (error)
    {
        throw error; 
    }
};
