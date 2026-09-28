import http from '@/http-common';

export default class LoginService {
    async Login(data){
        let response = await http.post(`api/v1/auth`, null, {
            params: { username: data.username, password: data.password }
        });
        localStorage.setItem("token", response.data.token);
        return response.data;
    }

    Logout(){
        localStorage.removeItem("token");
    }

    IsLogged(){
        return !!localStorage.getItem("token");
    }
}
