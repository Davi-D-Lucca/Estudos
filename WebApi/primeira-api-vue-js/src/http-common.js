import Axios from 'axios';

const createAxios = Axios.create({
    baseURL: "https://localhost:7267"
});

// Envia o token JWT em todas as requisicoes, se o usuario estiver logado
createAxios.interceptors.request.use((config) => {
    const token = localStorage.getItem("token");
    if (token) {
        config.headers.Authorization = `Bearer ${token}`;
    }
    return config;
});

// Token expirado ou invalido: limpa e avisa o App para voltar ao login
createAxios.interceptors.response.use(
    (response) => response,
    (error) => {
        if (error.response?.status === 401) {
            localStorage.removeItem("token");
            window.dispatchEvent(new Event("logout"));
        }
        return Promise.reject(error);
    }
);

export default createAxios;
