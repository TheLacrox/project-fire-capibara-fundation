#!/bin/sh
set -e

# Base args: baked config + persistent data dir on the volume.
set -- --config-file /app/server_config.toml --data-dir /data

# Always-on hardening (defense in depth; also set in the TOML).
# Behind a reverse proxy every connection looks like loopback, so loopback admin
# would hand full host to anyone who can reach the proxy.
set -- "$@" --cvar "console.loginlocal=false"

# Optional env overrides. Unset vars are skipped so the baked TOML value wins.
[ -n "$SS14_HOSTNAME" ]      && set -- "$@" --cvar "game.hostname=$SS14_HOSTNAME"
[ -n "$SS14_DESC" ]          && set -- "$@" --cvar "game.desc=$SS14_DESC"
[ -n "$SS14_HUB_ADVERTISE" ] && set -- "$@" --cvar "hub.advertise=$SS14_HUB_ADVERTISE"
[ -n "$SS14_AUTH_MODE" ]     && set -- "$@" --cvar "auth.mode=$SS14_AUTH_MODE"
[ -n "$SS14_HOST_USER" ]     && set -- "$@" --cvar "console.login_host_user=$SS14_HOST_USER"
[ -n "$SS14_SOFT_MAX" ]      && set -- "$@" --cvar "game.soft_max_players=$SS14_SOFT_MAX"

# Launcher "View server info" buttons.
[ -n "$SS14_DISCORD" ] && set -- "$@" --cvar "infolinks.discord=$SS14_DISCORD"
[ -n "$SS14_WEBSITE" ] && set -- "$@" --cvar "infolinks.website=$SS14_WEBSITE"
[ -n "$SS14_WIKI" ]    && set -- "$@" --cvar "infolinks.wiki=$SS14_WIKI"

# TTS. This fork talks to an HTTP TTS API (tts.api_url + tts.api_token); it does
# not use the redis/worker setup that other forks use. The token is confidential,
# so it must come from the environment and never from the baked TOML.
[ -n "$SS14_TTS_ENABLED" ]   && set -- "$@" --cvar "tts.enabled=$SS14_TTS_ENABLED"
[ -n "$SS14_TTS_API_URL" ]   && set -- "$@" --cvar "tts.api_url=$SS14_TTS_API_URL"
[ -n "$SS14_TTS_API_TOKEN" ] && set -- "$@" --cvar "tts.api_token=$SS14_TTS_API_TOKEN"

# Domain-derived launcher routing (HTTPS status via the proxy, UDP gameplay direct).
if [ -n "$SS14_DOMAIN" ]; then
  set -- "$@" --cvar "hub.server_url=ss14s://$SS14_DOMAIN"
  set -- "$@" --cvar "status.connectaddress=udp://$SS14_DOMAIN:1212"
fi

# Deliberately does not echo the argument list: it would leak tts.api_token to the logs.
echo "Starting Robust.Server (config=/app/server_config.toml data-dir=/data)"
exec ./Robust.Server "$@"
