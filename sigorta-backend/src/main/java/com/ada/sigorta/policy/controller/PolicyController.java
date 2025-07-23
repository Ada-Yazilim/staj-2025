package com.ada.sigorta.policy.controller;

import com.ada.sigorta.policy.dto.PolicyRequest;
import com.ada.sigorta.policy.dto.PolicyResponse;
import com.ada.sigorta.policy.service.PolicyService;
import lombok.RequiredArgsConstructor;
import org.springframework.security.core.Authentication;
import org.springframework.web.bind.annotation.*;

import java.util.List;

@RestController
@RequestMapping("/api/policies")
@RequiredArgsConstructor
public class PolicyController {

    private final PolicyService policyService;

    @PostMapping
    public void createPolicy(@RequestBody PolicyRequest request, Authentication authentication) {
        policyService.createPolicy(request, authentication);
    }

    @GetMapping("/me")
    public List<PolicyResponse> getMyPolicies(Authentication authentication) {
        return policyService.getMyPolicies(authentication);
    }
}
