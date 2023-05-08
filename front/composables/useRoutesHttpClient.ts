import {ofetch} from "ofetch";
import RouteModel from "~/models/route/RouteModel";

export default function () {
    const runtimeConfig = useRuntimeConfig()

    const httpClient = ofetch.create({
        baseURL: runtimeConfig.public.apiBaseUrl
    })

    const list = async (): Promise<RouteModel[]> => {
        return await httpClient<RouteModel[]>("/routes", {
            method: "get"
        })
    }

    return {
        list
    }
}