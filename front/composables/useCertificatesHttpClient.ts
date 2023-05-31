import {ofetch} from "ofetch";
import ListCertificateModel from "~/models/certificate/ListCertificateModel";
import CreateManagedCertificateModel from "~/models/certificate/CreateManagedCertificateModel";
import UploadCertificateModel from "~/models/certificate/UploadCertificateModel";

export default function () {
    const runtimeConfig = useRuntimeConfig();

    const httpClient = ofetch.create({
        baseURL: runtimeConfig.public.apiBaseUrl
    });

    const list = async (): Promise<ListCertificateModel[]> => {
        return await httpClient<ListCertificateModel[]>("/certificates", {
            method: "get"
        });
    }

    const upload = async (model: UploadCertificateModel): Promise<any> => {
        return await httpClient("/certificates/unmanaged", {
            method: "post",
            body: JSON.stringify(model)
        })
    }
    
    const createManaged = async (model: CreateManagedCertificateModel): Promise<any> => {
        return await httpClient("/certificates/managed", {
            method: "post",
            body: JSON.stringify(model)
        })
    }

    const delete_ = async (id: string): Promise<any> => {
        return await httpClient(`/certificates/${id}`, {
            method: "delete"
        });
    };

    return {
        list,
        createManaged,
        upload,
        delete_
    };
}