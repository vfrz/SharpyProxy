import {ofetch} from "ofetch";
import ClusterModel from "~/models/cluster/ClusterModel";

export default function () {
    const runtimeConfig = useRuntimeConfig()

    const httpClient = ofetch.create({
        baseURL: runtimeConfig.public.apiBaseUrl
    })

    const list = async (): Promise<ClusterModel[]> => {
        return await httpClient<ClusterModel[]>("/clusters", {
            method: "get"
        })
    }

    return {
        list
    }
}