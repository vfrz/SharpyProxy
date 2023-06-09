export default interface ListRouteModel {
    id: string,
    name: string,
    matchPath?: string,
    matchHosts: string[],
    clusterId: string,
    clusterName: string,
    clusterEnabled: boolean,
    enabled: boolean
}