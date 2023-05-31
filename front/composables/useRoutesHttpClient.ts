import {ofetch} from "ofetch";
import ListRouteModel from "~/models/route/ListRouteModel";
import CreateRouteModel from "~/models/route/CreateRouteModel";
import UpdateRouteModel from "~/models/route/UpdateRouteModel";
import ApiResponse from "~/models/ApiResponse";
import RouteModel from "~/models/route/RouteModel";

export default function () {
    const runtimeConfig = useRuntimeConfig();

    const httpClient = ofetch.create({
        baseURL: runtimeConfig.public.apiBaseUrl
    });
    
    const get = async (id: string): Promise<RouteModel> => {
        return await httpClient<RouteModel>(`/routes/${id}`, {
            method: "get"
        });
    };
    
    const create = async (model: CreateRouteModel): Promise<ApiResponse<string>> => {
        return await httpClient<ApiResponse<string>>("/routes", {
            method: "post",
            body: JSON.stringify(model)
        });
    };

    const update = async (model: UpdateRouteModel): Promise<ApiResponse> => {
        return await httpClient("/routes", {
            method: "put",
            body: JSON.stringify(model)
        });
    };

    const list = async (): Promise<ListRouteModel[]> => {
        return await httpClient<ListRouteModel[]>("/routes", {
            method: "get"
        })
    };

    const delete_ = async (id: string): Promise<any> => {
        return await httpClient(`/routes/${id}`, {
            method: "delete"
        });
    };

    return {
        get,
        create,
        update,
        list,
        delete_
    };
}