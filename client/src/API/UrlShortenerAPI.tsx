import axios from "axios";

const BASE_URL = process.env.REACT_APP_URL_SHORTENER_API_BASE_URL;

const api = axios.create({baseURL : BASE_URL});

export const shortenUrl = async (urlToShorten : string, customAlias? : string) : Promise<string>  => {
    
    try {
            const response = await api.post('/shorten',
                                              {
                                                  longUrl: urlToShorten,
                                                  customAlias: customAlias || undefined
                                              });

            return response.data;

        }
        
        catch (error) {
            console.error(error);
            throw error; 
        }
    
}

