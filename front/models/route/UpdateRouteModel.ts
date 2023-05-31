export default interface UpdateRouteModel {
    id: string,
    name: string,
    enabled: boolean,
    matchPath?: string,
    matchHosts?: string[],
    clusterId: string
}