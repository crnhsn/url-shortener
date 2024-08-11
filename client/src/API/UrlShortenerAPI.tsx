import axios, { AxiosResponse, AxiosError } from "axios";

const BASE_URL = "http://localhost:3000";

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
