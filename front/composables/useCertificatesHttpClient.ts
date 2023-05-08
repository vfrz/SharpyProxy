import {ofetch} from "ofetch";
import ListCertificateModel from "~/models/certificate/ListCertificateModel";

export default function () {
    const runtimeConfig = useRuntimeConfig()

    const httpClient = ofetch.create({
        baseURL: runtimeConfig.public.apiBaseUrl
    })

    const list = async (): Promise<ListCertificateModel[]> => {
        return await httpClient<ListCertificateModel[]>("/certificates", {
            method: "get"
        })
    }

    return {
        list
    }
}