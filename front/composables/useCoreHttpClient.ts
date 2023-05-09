import {ofetch} from "ofetch";
import StatsModel from "~/models/core/StatsModel";

export default function () {
    const runtimeConfig = useRuntimeConfig()

    const httpClient = ofetch.create({
        baseURL: runtimeConfig.public.apiBaseUrl
    })

    const getStats = async (): Promise<StatsModel> => {
        return await httpClient<StatsModel>("/core/stats", {
            method: "get"
        })
    }

    return {
        getStats
    }
}