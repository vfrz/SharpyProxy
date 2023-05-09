import {ofetch} from "ofetch";
import ClusterModel from "~/models/cluster/ClusterModel";
import CreateClusterModel from "~/models/cluster/CreateClusterModel";

export default function () {
    const runtimeConfig = useRuntimeConfig()

    const httpClient = ofetch.create({
        baseURL: runtimeConfig.public.apiBaseUrl
    })

    const create = async (model: CreateClusterModel): Promise<string> => {
        return await httpClient<string>("/clusters", {
            method: "post",
            body: JSON.stringify(model)
        })
    }
    
    const list = async (): Promise<ClusterModel[]> => {
        return await httpClient<ClusterModel[]>("/clusters", {
            method: "get"
        })
    }

    return {
        create,
        list
    }
}