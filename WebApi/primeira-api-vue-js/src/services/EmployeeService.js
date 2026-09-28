import http from '@/http-common';

export default class EmployeeService {
    constructor(version = "v1"){
        this.version = version;
    }

    get baseUrl(){
        return `api/${this.version}/employee`;
    }

    // GET api/v{n}/employee?pageNumber=&pageQuantity=  (pageNumber comeca em 0)
    async Get(pageNumber, pageQuantity){
        let response = await http.get(this.baseUrl, {
            params: { pageNumber, pageQuantity }
        });
        return response.data;
    }

    // GET api/v{n}/employee/{id}  (204 sem corpo quando nao existe)
    async Search(id){
        let response = await http.get(`${this.baseUrl}/${id}`);
        return response.data || null;
    }

    // POST api/v{n}/employee  (multipart/form-data, precisa de token)
    async Add(employee){
        let form = new FormData();
        form.append("Name", employee.name);
        form.append("Age", employee.age);
        form.append("Photo", employee.photo);
        await http.post(this.baseUrl, form);
    }

    // POST api/v{n}/employee/{id}/download  (precisa de token) -> URL local da imagem
    async DownloadPhoto(id){
        let response = await http.post(`${this.baseUrl}/${id}/download`, null, {
            responseType: "blob"
        });
        return URL.createObjectURL(response.data);
    }
}
