import {ofetch} from "ofetch";
import ClusterModel from "~/models/cluster/ClusterModel";
import CreateClusterModel from "~/models/cluster/CreateClusterModel";
import UpdateClusterModel from "~/models/cluster/UpdateClusterModel";
import ApiResponse from "~/models/ApiResponse";

export default function () {
    const runtimeConfig = useRuntimeConfig();

    const httpClient = ofetch.create({
        baseURL: runtimeConfig.public.apiBaseUrl
    });

    const create = async (model: CreateClusterModel): Promise<ApiResponse<string>> => {
        return await httpClient<ApiResponse<string>>("/clusters", {
            method: "post",
            body: JSON.stringify(model)
        });
    }

    const update = async (model: UpdateClusterModel): Promise<any> => {
        return await httpClient("/clusters", {
            method: "put",
            body: JSON.stringify(model)
        });
    }

    const get = async (id: string): Promise<ClusterModel> => {
        return await httpClient(`/clusters/${id}`, {
            method: "get"
        });
    }

    const delete_ = async (id: string): Promise<any> => {
        return await httpClient(`/clusters/${id}`, {
            method: "delete"
        });
    }

    const list = async (): Promise<ClusterModel[]> => {
        return await httpClient<ClusterModel[]>("/clusters", {
            method: "get"
        });
    }

    return {
        create,
        update,
        get,
        delete_,
        list
    }
}