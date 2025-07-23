package com.ada.sigorta.policy.service;

import com.ada.sigorta.customer.model.Customer;
import com.ada.sigorta.customer.repository.CustomerRepository;
import com.ada.sigorta.policy.dto.PolicyRequest;
import com.ada.sigorta.policy.dto.PolicyResponse;
import com.ada.sigorta.policy.model.Policy;
import com.ada.sigorta.policy.repository.PolicyRepository;
import com.ada.sigorta.user.model.Users;
import com.ada.sigorta.user.repository.UserRepository;
import lombok.RequiredArgsConstructor;
import org.springframework.security.core.Authentication;
import org.springframework.stereotype.Service;

import java.util.List;

import static java.util.stream.Collectors.toList;

@Service
@RequiredArgsConstructor
public class PolicyService {

    private final PolicyRepository policyRepository;
    private final UserRepository userRepository;
    private final CustomerRepository customerRepository;

    public void createPolicy(PolicyRequest request, Authentication authentication) {
        Users user = userRepository.findByUsername(authentication.getName()).orElseThrow();
        Customer customer = customerRepository.findByUser(user);

        Policy policy = Policy.builder()
                .type(request.getType())
                .startDate(request.getStartDate())
                .endDate(request.getEndDate())
                .premium(request.getPremium())
                .isPaid(false)
                .customer(customer)
                .build();

        policyRepository.save(policy);
    }

    public List<PolicyResponse> getMyPolicies(Authentication authentication) {
        Users user = userRepository.findByUsername(authentication.getName()).orElseThrow();
        Customer customer = customerRepository.findByUser(user);

        return policyRepository.findByCustomer(customer).stream().map(policy ->
                PolicyResponse.builder()
                        .id(policy.getId())
                        .type(policy.getType())
                        .startDate(policy.getStartDate())
                        .endDate(policy.getEndDate())
                        .premium(policy.getPremium())
                        .isPaid(policy.isPaid())
                        .build()
        ).collect(toList());
    }
}
