import {ofetch} from "ofetch";
import ListRouteModel from "~/models/route/ListRouteModel";

export default function () {
    const runtimeConfig = useRuntimeConfig()

    const httpClient = ofetch.create({
        baseURL: runtimeConfig.public.apiBaseUrl
    })

    const list = async (): Promise<ListRouteModel[]> => {
        return await httpClient<ListRouteModel[]>("/routes", {
            method: "get"
        })
    }

    return {
        list
    }
}