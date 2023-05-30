import {ofetch} from "ofetch";
import ListRouteModel from "~/models/route/ListRouteModel";
import CreateRouteModel from "~/models/route/CreateRouteModel";
import ApiResponse from "~/models/ApiResponse";

export default function () {
    const runtimeConfig = useRuntimeConfig();

    const httpClient = ofetch.create({
        baseURL: runtimeConfig.public.apiBaseUrl
    });
    
    const create = async (model: CreateRouteModel): Promise<ApiResponse<string>> => {
        return await httpClient<ApiResponse<string>>("/routes", {
            method: "post",
            body: JSON.stringify(model)
        });
    };

    const list = async (): Promise<ListRouteModel[]> => {
        return await httpClient<ListRouteModel[]>("/routes", {
            method: "get"
        })
    };

    return {
        create,
        list
    };
}