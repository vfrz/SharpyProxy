export default interface RouteModel {
    id: string,
    name: string,
    matchPath?: string,
    matchHosts: string[],
    clusterId: string,
    enabled: boolean
}